using UnityEngine;
using RR.Movement;
using RR.Combat;
using RR.Core;
using System;
using UnityEngine.AI;

namespace RR.Control
{
    public class AIController : MonoBehaviour 
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float _suspicionTime = 10f;
        [SerializeField] PatrolPath _patrolPath;
        [SerializeField] float waypointTolerance = 1.0f;
        [SerializeField] float waypointDwellTime = 3.0f;

        [Range(0,1)]//This limits the range of the patrolSpeedFraction value below.
        [SerializeField] float patrolSpeedFraction = 0.2f;//Percent of max speed in Mover.cs to apply for waling.
        Mover _mover;
        Fighter _fighter;
        GameObject player;
        Health _health;
        Vector3 _guardPosition;
        float _timeSinceLastSawPlayer = Mathf.Infinity;//Set to a long time ago so can reset for AI immediately on start;
        float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;//Start at waypoint 1
        ActionScheduler _actionScheduler;
        #region
        void Start()
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
            if(_actionScheduler)
            {
                Debug.LogError("ActionScheduler component not found");
            }
        }
        #endregion

        private void Update()
        {
            if (_health.IsDead()) return;
            if (InAttackRangeOfPlayer() && _fighter.CanAttack(player))
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
            //_mover.MoveTo(player.transform.position);
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(player);
        }

        bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
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