using UnityEngine;

namespace RR.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] float speed = 2f;
        Rigidbody rb;
        ///// Example similar to Sharp Accent https://www.youtube.com/watch?v=gARh1de-obM

        void Start()
        {
            rb = GetCompoonent<Rigidbody>();
            if(rb == null) return;
        }
        
        void FixedUpdate()
        {
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }
    }
}