using UnityEngine;

namespace RR.Combat
{
    public class EnablePickup : MonoBehaviour
    {
        WeaponPickup _weaponPick;

        void Start()
        {
            _weaponPick = GetComponent<WeaponPickup>();
        }
        
        void OnTriggerExit(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                _weaponPick.enabled = true;
            }
        }
    }
}