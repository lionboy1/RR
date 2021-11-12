using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RR.CameraScripts
{
    //Standard billboard script
    //Add to the target canvas to make it always face the camera.
    public class Billboard : MonoBehaviour
    {
        Transform cam;
      
        void Start()
        {
            cam = GameObject.Find("Camera").transform;
        }
        // Wait for other frames to update before updating camera
        void LateUpdate()
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}


