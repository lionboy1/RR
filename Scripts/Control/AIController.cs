using UnityEngine;
using RR.Movement;
using RR.Combat;
using RR.Core;
using System.Collections.Generic;

namespace RR.Control
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(ActionScheduler))]
    public abstract class AIController : MonoBehaviour 
    {
        [SerializeField] protected float chaseDistance;
        [SerializeField] protected float _suspicionTime;
        //Make the aggro time higher than suspicion time
        [SerializeField] protected float _aggroCooldownTime;
        [SerializeField] PatrolPath _patrolPath;
        [SerializeField] protected float waypointTolerance = 1.0f;
        [SerializeField] protected float waypointDwellTime = 3.0f;

        [Range(0,1)]//This limits the range of the patrolSpeedFraction value below.
        [SerializeField] float patrolSpeedFraction = 0.2f;//Percent of max speed in Mover.cs to apply for waling.
        [SerializeField] float shoutDistance;
        Mover _mover;
        Fighter _fighter;
        PlayerController _playerController;

        [SerializeField] protected GameObject player;
        // [SerializeField] protected List<GameObject> closestPlayer = new List<GameObject>();

        [SerializeField] protected GameObject[] attackablesPC;
        [SerializeField] protected GameObject[] attackablesNPC;

        Health _health;
        Vector3 _guardPosition;
        protected float _timeSinceLastSawPlayer = Mathf.Infinity;//Set to a long time ago so can reset for AI immediately on start;
        protected float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        protected float timeSinceAggravated = Mathf.Infinity;
        int currentWaypointIndex = 0;//Start at waypoint 1
        ActionScheduler _actionScheduler;
        #region
        void Awake()
        {
            //Wherever transform is originally
            //placed that is where it will guard.
            _guardPosition = transform.position;
            
            _mover = GetComponent<Mover>();
            if(_mover == null)
            {
                Debug.LogError("Move component not found!");
            }
            _fighter = GetComponent<Fighter>();
            if(_fighter == null)
            {
                Debug.LogError("Fighter component not found!");
            }


            /*player = GameObject.FindWithTag("Player");
            if(player == null)
            {
                Debug.LogError("Player game object not found");
            }*/
            attackablesPC = GameObject.FindGameObjectsWithTag("Player");
            attackablesNPC = GameObject.FindGameObjectsWithTag("NPC");
            
            _health = GetComponent<Health>();
            if(_health == null)
            {
                Debug.LogError("Health component not found");
            }
            _actionScheduler = GetComponent<ActionScheduler>();
            if(_actionScheduler == null)
            {
                Debug.LogError("ActionScheduler component not found");
            }
            //Define a random enemy to avoid errors
            //this will update based on nearest enemy calculations in EnemyFindClosestPlayerToAttack()
            player = attackablesPC[Random.Range(0, 1)];
        }

        #endregion

        private void Update()
        {
            if (_health.IsDead()) return;
            if (IsAggravated() && _fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (_timeSinceLastSawPlayer < _suspicionTime)
            {
                //Suspiscion state
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();//will automaticall cancel current action.
            }
            UpdateTimers();
            EnemyFindClosestPlayerToAttack();
        }


        //WIP - find closest go tagged "Player"
        //currently other gos tagges as "Player" are detected, however...
        //when they are attacked the damage still transfers to the main player character..needs fixing
        void EnemyFindClosestPlayerToAttack()
        {
            foreach(GameObject pcClosest in attackablesPC)
            {
                float dist = Vector3.Distance(this.transform.position, pcClosest.transform.position);
                {
                    if(dist < chaseDistance)
                    {
                        //The player is whatever the closest go is
                        
                        if(pcClosest.GetComponent<PlayerController>().InteractWithCombat())
                        {
                            player = pcClosest;
                            Health newTargetPC  = pcClosest.GetComponent<Health>();
                            _fighter.target = newTargetPC;
                            _fighter.CanAttack(pcClosest);
                        }
                    }
                    /*else if(dist > chaseDistance)
                    {
                        foreach(GameObject npcClosest in attackablesNPC)
                        {
                            if(dist < chaseDistance )
                            {
                                player = npcClosest;
                                Health newTargetNPC  = npcClosest.GetComponent<Health>();
                                _fighter.target = newTargetNPC;
                                _fighter.CanAttack(npcClosest);
                                
                                
                            }
                        }
                    }*/
                }
            }
        }

        //Can call this directly on neutral npcs
        //who should only attack when attacked first
        //Do not have them start the game being aggressive
        //Potentially implement reputation system, allowing them to remember past
        //attacks from player to make them aggressive long term.
        public void Aggravate()
        {
            timeSinceAggravated = 0;
        }

        public virtual bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Check if aggravation timer has expired
            return distanceToPlayer < chaseDistance || timeSinceAggravated <_aggroCooldownTime;
        }

        private void UpdateTimers()
        {
            timeSinceAggravated += Time.time;//delta time increment too sall and wasn't working
            _timeSinceLastSawPlayer += Time.deltaTime;//Count time since last saw.
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        //Nov 2021 update - completely new idea
        //When hurt, the npc will retreat to the nearest hideout - WIP
        void RetreatBehavior()
        {
            _actionScheduler.CancelCurrentAction();
            //Then find nearest hideout
        }
        private void SuspicionBehavior()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = _guardPosition;
            if(_patrolPath != null)
            {
                if(AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(_timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = _patrolPath.GetNextWayIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;//Return true only when within range.
        }

        void AttackBehavior()
        {
            
            // EnemyFindClosestPlayerToAttack();
            _mover.MoveTo(player.transform.position, 1);
            //Reset time since just saw before attacking.
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(player);

            AggravateNearbyEnemies();
        }

        //Option 1. Make this virtual so only similar species can call each other for help
        //Option 2. Try using an enum to set species then they only call to each other.
        void AggravateNearbyEnemies()
        {
            
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);//No direction for sphere, it surounds the GameObject.
            foreach(RaycastHit hit in hits)
            {
                AIController enemy = hit.transform.GetComponent<AIController>();
                if(enemy == null) continue;
                //else
                enemy.Aggravate();
            }
        }

        
        //Called by Unity
        //Creates a gizmo to see enemy radius
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);//position and radius.
        }

    }
}