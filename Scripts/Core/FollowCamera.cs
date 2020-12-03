using UnityEngine;

namespace RR.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        
    
        // Update is called once per frame
        void LateUpdate()
        {
            this.transform.position = target.position;
            //float horizontalInput = Input.GetAxis("Horizontal");
            
        }
        
    }
}

