using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RR.Core;

namespace RR.Combat
{
    public class WeaponDamage : MonoBehaviour
    {
        Health playerHealth;
        // Fighter playerFighterComp;
        [SerializeField] float damageAmount;
        // Start is called before the first frame update
        void Start()
        {
            playerHealth = GameObject.Find("Player").GetComponent<Health>();
            if(playerHealth == null)
            {
                Debug.LogError("No player health refreence for enemy to attack");
            }
            /*playerFighterComp = GameObject.Find("Player").GetComponent<Fighter>();
            if(playerFighterComp == null)
            {
                Debug.LogError("No player fighter component ref found to attack.");
            }*/
        }

        // Update is called once per frame
        void Update()
        {

        }


        void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player")
            {
                //call damage on player
                // playerFighterComp.Hit();
                playerHealth.Damage(damageAmount);
            }
        }
    }
}    
