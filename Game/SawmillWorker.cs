using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SawmillWorker : Worker
{
    //Saw mill worker processes logs into planks
   public override HashSet<KeyValuePair<string,object>> CreateGoalState ()
	{
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		goal.Add(new KeyValuePair<string, object>("makeLumber", true ));

		return goal;
	}

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
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
