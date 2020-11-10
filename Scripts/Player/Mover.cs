using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Mover : MonoBehaviour
{
    
    [SerializeField]
    Transform target;

    NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if(_agent == null)
        {
            Debug.LogError("NAvMeshAgent not attached");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }
        //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
        UpdateAnimator();
        _agent.destination = target.position;
    }

    void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if( hasHit)
        {
            _agent.destination = hit.point;
        }
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
