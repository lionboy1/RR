using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RR.Core;

namespace RR.Combat
{
    public class EnemyWeaponDamage : MonoBehaviour
    {
        [SerializeField] float damageAmount;

        // Update is called once per frame

        void Update()
        {

        }

        void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player" || col.tag == "NPC"  )
            {
                col.GetComponent<Health>().Damage(damageAmount);
            }
        }
    }
}    
