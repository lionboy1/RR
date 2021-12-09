using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RR.Inventories.Equipment;

namespace RR.Inventories.Equipment
{
    public class EquipmentManager : MonoBehaviour
    {
        public static EquipmentManager instance;

        #region Singleton

        void Awake()
        {
            if(instance != null)
            {
                Debug.Log("Instance of Equipment Manager already running!");
            }
            instance = this;
        }

        #endregion

        Equipment[] currentEquipment;

        void Start()
        {
            //Trick to grab the number of string elements in an enum. A string array
            int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[numSlots];
        }

        public void Equip(Equipment newItem)
        {
            int slotIndex = (int)newItem.equipSlot;

            currentEquipment[slotIndex] = newItem;
        }
    }
}


