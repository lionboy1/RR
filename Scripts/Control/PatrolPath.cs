using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RR.Control
{
    public class PatrolPath : MonoBehaviour
    {
    //Create an empty called PatrolPath and child waypoints to it
        const float waypointGizmoRadius = 0.3f;
        void OnDrawGizmos()
        {
            //Using for loop to get index value.  foreach refrences gameobjects etc
            for(int i = 0; i < transform.childCount; i++)
            {
                GetNextWayIndex(i);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                //Gizmos.DrawLine()
            }
        }

        //Counts each waypoint and when it reaches the last index, resets to zero to start 
        //at the first waypoint again.
        public int GetNextWayIndex(int i)
        {
            if( i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

