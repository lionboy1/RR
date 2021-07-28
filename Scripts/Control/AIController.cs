using UnityEngine;
using RR.Movement;
using RR.Combat;
using RR.Core;
using System;
using UnityEngine.AI;

namespace RR.Control
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(ActionScheduler))]
    public class AIController : MonoBehaviour 
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float _suspicionTime = 10f;
        [SerializeField] float _aggroCooldownTime = 10f;
        [SerializeField] PatrolPath _patrolPath;
        [SerializeField] float waypointTolerance = 1.0f;
        [SerializeField] float waypointDwellTime = 3.0f;

        [Range(0,1)]//This limits the range of the patrolSpeedFraction value below.
        [SerializeField] float patrolSpeedFraction = 0.2f;//Percent of max speed in Mover.cs to apply for waling.
        [SerializeField] float shoutDistance = 5f;
        Mover _mover;
        Fighter _fighter;
        GameObject player;
        Health _health;
        Vector3 _guardPosition;
        float _timeSinceLastSawPlayer = Mathf.Infinity;//Set to a long time ago so can reset for AI immediately on start;
        float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggravated = Mathf.Infinity;
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
            player = GameObject.FindWithTag("Player");
            if(player == null)
            {
                Debug.LogError("Player game object not found");
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
                PatrolBehavior();//will automaticall cancel currenc action.
            }
            UpdateTimers();
        }

        public void Aggravate()
        {
            timeSinceAggravated = 0;
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;//Count time since last saw.
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
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

        private void AttackBehavior()
        {
            //Reset time since just saw before attacking.
            _mover.MoveTo(player.transform.position, 1);
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(player);

            AggravateNearbyEnemies();
        }

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

        bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Check if aggravation timer has expired
            return distanceToPlayer < chaseDistance || timeSinceAggravated <_aggroCooldownTime;
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