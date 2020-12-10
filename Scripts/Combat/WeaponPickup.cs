using System.Collections;
using UnityEngine;

namespace RR.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        Fighter fighter;
        [SerializeField] Weapon weapon = null;

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Debug.Log("Picking up spear!") ;
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(this.gameObject);
            }
        }
    }
}