using RR.Core;
using UnityEngine;

namespace RR.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] float speed = 2f;
        Health target = null;
        float rayLength = 2f;
        Rigidbody rb;
        
        #region
        ///// Example similar to Sharp Accent https://www.youtube.com/watch?v=gARh1de-obM
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if(rb == null) return;
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }
        
        void FixedUpdate()
        {
            LayerMask mask = LayerMask.GetMask("Enemy");
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayLength, mask));
            {
                rb.isKinematic = true;
                transform.parent = hit.transform.parent;
                this.enabled = false;
            }
        }
        #endregion

        public void SetTarget(Health target)
        {
            this.target = target;
        }
    }
}