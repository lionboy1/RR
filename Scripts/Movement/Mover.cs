//using RR.Combat;
using UnityEngine.EventSystems;
using RR.Core;
using UnityEngine;
using UnityEngine.AI;
using RR.Saving;

namespace RR.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ActionScheduler))]
    public class Mover : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] Transform target;
        [SerializeField] float _maxSpeed = 5f;
        NavMeshAgent _agent;
        ActionScheduler _actionScheduler;
        Health _health;
                  
       void Awake()
        {
            //yield return new WaitForSeconds(5);
            _agent = GetComponent<NavMeshAgent>();
            if(_agent == null)
            {
                Debug.LogError("NavMeshAgent component not found");
            }
            
            _actionScheduler = GetComponent<ActionScheduler>();
            if(_actionScheduler == null)
            {
                Debug.LogError("ActionScheduler component not found");
            }

            _health = GetComponent<Health>();
            if(_health == null)
            {
                Debug.LogError("Health component not found");
            }
        }                  
        // Update is called once per frame
        void Update()
        {
            _agent.enabled = !_health.IsDead();//so only if the character isn't dead will the nav mesh agent be enabled.
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
             _agent.isStopped = false;
            _agent.destination = destination;
            _agent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);//Clamp01 only accepts numbers between o and 1.  This protects against numeric overload.
        }

        public void Cancel() 
        {
            _agent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            //If inventory is open, avoid player movement when clicking on UI elements
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //Otherwise, proceed to action
            _actionScheduler.StartAction(this);//takes care of starting and stopping mover/fighter components
            MoveTo(destination, speedFraction);
        }

        void UpdateAnimator()
        {
            //First get global velocity on navmesh agent
            Vector3 velocity = _agent.velocity;
            //convert to local velocity
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            //Which direction of interest for movement
            float speed = localVelocity.z;
            //Influence the float parameter on the animator by feding it the speed values from the local velocity.
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;
            //Disable nav mesh agent from interferring while positions are loading
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            //Re-enable nav agent after world positions are loaded
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
