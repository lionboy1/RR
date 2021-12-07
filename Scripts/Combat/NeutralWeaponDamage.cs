using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RR.Core;

namespace RR.Combat
{
    public class NeutralWeaponDamage : MonoBehaviour
    {
        [SerializeField] float damageAmount;

        // Update is called once per frame

        void Update()
        {

        }

        void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player" || col.GetInstanceID() != this.gameObject.GetInstanceID()  )
            {
                col.GetComponent<Health>().Damage(damageAmount);
            }
        }
    }
}    
