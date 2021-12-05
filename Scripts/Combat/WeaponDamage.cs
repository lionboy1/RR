using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RR.Core;

namespace RR.Combat
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] float damageAmount;

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player" || col.tag == "NPC")
            {
                //call damage on player
                // playerFighterComp.Hit();
                col.GetComponent<Health>().Damage(damageAmount);
                // playerHealth.Damage(damageAmount);
            }
        }
    }
}    
