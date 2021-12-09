using UnityEngine;
using RR.Movement;
using RR.Combat;
using RR.Core;
using RR.Environment;
using System.Collections.Generic;
using UnityEngine.AI;

namespace RR.Control
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(ActionScheduler))]
    public abstract class AIController : MonoBehaviour 
    {
        #region
        [SerializeField] protected float chaseDistance;
        [SerializeField] protected float _suspicionTime;
        //Make the aggro time higher than suspicion time
        [SerializeField] protected float _aggroCooldownTime;
        [SerializeField] PatrolPath _patrolPath;
        [SerializeField] protected float waypointTolerance = 1.0f;
        [SerializeField] protected float waypointDwellTime = 3.0f;

        [Range(0,1)]//This limits the range of the patrolSpeedFraction value below.
        [SerializeField] float patrolSpeedFraction = 0.2f;//Percent of max speed in Mover.cs to apply for waling.
        [SerializeField] protected float shoutDistance;
        Mover _mover;
        Fighter _fighter;
        PlayerController _playerController;

        [SerializeField] public GameObject player;
        [SerializeField] public GameObject candidate;
        [SerializeField] public GameObject currentHidingSpot;
        [SerializeField] public GameObject hide;
        // [SerializeField] protected List<GameObject> closestPlayer = new List<GameObject>();

        [SerializeField] protected GameObject[] attackablesPC;
        [SerializeField] protected GameObject[] attackableBuildings;
        [SerializeField] protected GameObject[] attackableNPC;
        [SerializeField] protected GameObject[] HidingPlaces;
        CharacterClasses charClass;

        protected Health _health;
        Vector3 _guardPosition;
        protected float _timeSinceLastSawPlayer = Mathf.Infinity;//Set to a long time ago so can reset for AI immediately on start;
        protected float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        protected float timeSinceAggravated = Mathf.Infinity;
        int currentWaypointIndex = 0;//Start at waypoint 1
        ActionScheduler _actionScheduler;
        CharacterClasses characterClasses;
        #endregion

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
            
        }

        void Start()
        {
            attackablesPC = GameObject.FindGameObjectsWithTag("Player");
            player = attackablesPC[Random.Range(0, 1)];
            
            
            attackableBuildings = GameObject.FindGameObjectsWithTag("Building");
            attackableNPC = GameObject.FindGameObjectsWithTag("NPC");
            HidingPlaces = GameObject.FindGameObjectsWithTag("HidingSpot");
        }

        #endregion

        private void Update()
        {
            if (_health.IsDead()) return;
            /*if(RetreatBehavior())
            {
                //Run and hide to recover
                // RetreatBehavior();
            }*/
            if(_health.LowHealth())
            {
                TestRetreatBehavior();
            }
            
            if (IsAggravated() && _fighter.CanAttack(player) && !_health.LowHealth())
            {
                AttackBehavior();
            }
            else if(FindClosestTargetToAttack())
            {
                //Attack each other
            }
            else if (_timeSinceLastSawPlayer < _suspicionTime)
            {
                //Suspiscion state
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();//will automatically cancel current action.
            }
            UpdateTimers();
        }
            
            void TestRetreatBehavior()
            {
                // _mover.StartMoveAction(hide.transform.position, 1);
                _fighter.target = null;
                _fighter.Cancel();
                _actionScheduler.CancelCurrentAction();
                _mover.MoveTo(hide.transform.position, 1);
            }
            bool FindClosestTargetToAttack()
            {
                /*if (Vector3.Distance(player.transform.position, transform.position) < chaseDistance && _fighter.CanAttack(player) || IsAggravated() && _fighter.CanAttack(player))
                {
                    _fighter.Attack(player);
                    // AttackBehavior();
                    return true;
                }*/

                GameObject candidate = null;
                float distance = chaseDistance;
                foreach (GameObject attackable in attackableNPC)
                {
                    if (attackable == gameObject) continue;
                    
                    float dist = Vector3.Distance(attackable.transform.position, transform.position);
                    if (dist < distance && _fighter.CanAttack(attackable))
                    {
                        candidate = attackable;
                        distance = dist;
                    }
                }

                if (candidate != null)
                {
                    _fighter.Attack(candidate);
                    return true;
                }
                return false;
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
            return distanceToPlayer < chaseDistance && timeSinceAggravated <_aggroCooldownTime;
        }
         void AttackBehavior()
        {
            _mover.MoveTo(player.transform.position, 1);
            //Reset time since just saw before attacking.
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(player);

            // AggravateNearbyEnemies();
        }

        public virtual void AggravateNearbyEnemies()
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

        private void UpdateTimers()
        {
            timeSinceAggravated += Time.time;//delta time increment too sall and wasn't working
            _timeSinceLastSawPlayer += Time.deltaTime;//Count time since last saw.
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        //Nov 2021 update - completely new idea
        //When hurt, the npc will retreat to the nearest hideout - WIP
        bool RetreatBehavior()
        {
            _actionScheduler.CancelCurrentAction();
            //Then find nearest hideout
            if(_health.LowHealth())
            {
                GameObject currentHidingSpot = null;
                foreach(GameObject place in HidingPlaces)
                {
                    float disToHide = Vector3.Distance(place.transform.position, transform.position);
                    if(disToHide < chaseDistance)
                    {
                        currentHidingSpot = place;
                        Debug.Log("Found " + currentHidingSpot.name + " to hide");

                        disToHide = chaseDistance;
                    }
                    if(currentHidingSpot != null)
                    {
                        _mover.MoveTo(currentHidingSpot.transform.position, 1);
                        Debug.Log("Moving to " + currentHidingSpot.name);
                        return true;
                    }
                }
                // return false;
            }
            return false;    
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

       

        
        //Called by Unity
        //Creates a gizmo to see enemy radius
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);//position and radius.
        }

    }
}