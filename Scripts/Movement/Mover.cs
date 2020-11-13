using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RR.Movement
{
    //[RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {

        [SerializeField]
        Transform target;

                  
        // Update is called once per frame
        void Update()
        {
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination;
        }

        void UpdateAnimator()
        {
            //First get global velocity on navmesh agent
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            //convert to local velocity
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            //Which direction of interest for movement
            float speed = localVelocity.z;
            //Influence the float parameter on the animator by feding it the speed values from the local velocity.
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}
