//using RR.Combat;
using RR.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RR.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ActionScheduler))]
    public class Mover : MonoBehaviour, IAction
    {

        [SerializeField]
        Transform target;

        NavMeshAgent _agent;
              
        ActionScheduler _actionScheduler;
                  
       void Start()
        {
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
        }                  
        // Update is called once per frame
        void Update()
        {
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
             _agent.isStopped = false;
            _agent.destination = destination;
        }

        public void Cancel() 
        {
            _agent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);//takes care of starting and stopping mover/fighter compoents
            MoveTo(destination);
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
    }
}
