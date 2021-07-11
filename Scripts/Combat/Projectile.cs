using RR.Core;
using UnityEngine;

namespace RR.Combat
{
    //[RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] GameObject _bloodSplashEffect;
        [SerializeField] float speed = 35f;

        Health target = null;
        float damage = 0;

        //float rayLength = 2f;
        //public Rigidbody rb;
        
        
        ///// Example similar to Sharp Accent https://www.youtube.com/watch?v=gARh1de-obM
        /*void Start()
        {
            rb = this.GetComponent<Rigidbody>();
            if(rb == null) return;
            //rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }*/
        
        public void SetTarget(Health target, float damage )
        {
            this.target = target;
            this.damage = damage;
        }

        

        /*void RaycastAim()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit, Mathf.Infinity)
            {
                //
            }

        }*/
        #region
        void Update()
        {
            if(target == null)
            {
                return;
            }
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        #endregion

        Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null)
            {
                return transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;

        }

        void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<Health>() != target)
            {
                return;
            }
            else
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                RaycastHit hitInfo;
                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {
                    
                    target.Damage(damage);
                    if(col.GetComponent<Health>().GetCurrentHealth() == 0)
                    {
                        GameObject _bloodSplashInstance = Instantiate(_bloodSplashEffect, target.transform.position, Quaternion.LookRotation(hitInfo.normal));
                    }
                    Destroy(this.gameObject);
                }
                
            }
        }


        #region
        /*void FixedUpdate()
        {
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
            //LayerMask mask = LayerMask.GetMask("Enemy");
            int bitmask = (1 << 13);//only te layer 13 has a binary 1 so it is the one ignored. Idea from code monkey.
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayLength, bitmask));
            {
                if(hit.collider != null)
                {
                    Debug.Log("Hitting enemy");
                }
                //rb.isKinematic = true;
                //this.transform.parent = hit.transform.parent;
                //this.enabled = false;
            }
        }*/
        #endregion
        //Will raycast in projectile.cs instead of homing.
    }
}