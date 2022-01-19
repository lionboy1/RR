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

        public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
        public OnEquipmentChanged onEquipmentChanged;
        Inventory inventory;
        Equipment[] currentEquipment;//Items we have currently equipped.
        SkinnedMeshRenderer[] currentMeshes;
        public SkinnedMeshRenderer targetMesh;
        public Equipment[] defaultItems;

        void Start()
        {
            inventory = Inventory.instance;
            
            //Trick to grab the number of string elements in an enum. A string array
            int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[numSlots];
            currentMeshes = new SkinnedMeshRenderer[numSlots];
            
            EquipDefaultItems();
        }

        public void Equip(Equipment newItem)
        {
            //Find out what slot the item fits into.
            int slotIndex = (int)newItem.equipSlot;
            Equipment oldItem = Unequip(slotIndex);

            //If there was already an item in the slot before equipping
            //place it back when it is swapped with a newly equipped item
            if(currentEquipment[slotIndex] != null)
            {
                oldItem = currentEquipment[slotIndex];
                inventory.Add(oldItem);
            }
            //If there are subscribers, notify them when an item is equipped
            if(onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(newItem, oldItem);
            }

            SetEquipmentBlendShapes(newItem, 100);
            //Insert the item into the slot
            currentEquipment[slotIndex] = newItem;
            SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
            //Parent the mesh to fit to the target
            newMesh.transform.parent = targetMesh.transform;
            //Now, deform the mesh to properly fit
            newMesh.bones = targetMesh.bones;
            newMesh.rootBone = targetMesh.rootBone;
            currentMeshes[slotIndex] = newMesh;

        }

        //Unequip a single item.  Will be called by a button
        public Equipment Unequip(int slotIndex)
        {
            //Only do this if the item is there
            if(currentEquipment[slotIndex] != null)
            {
                //If there is a current equipment item
                //at the target slot, destroy it when unequipped
                if(currentMeshes[slotIndex] != null)
                {
                    Destroy(currentMeshes[slotIndex].gameObject);
                }
                //Add the item back to the inventory
                Equipment oldItem = currentEquipment[slotIndex];
                //Set blendshape weight back to 0 at area where it was prev. equipped
                SetEquipmentBlendShapes(oldItem, 0);
                inventory.Add(oldItem);
                
                //Remove the item from the equipment array
                currentEquipment[slotIndex] = null;
                //If there are subscribers, notify them of equipment change
                if(onEquipmentChanged != null)
                {
                    onEquipmentChanged.Invoke(null, oldItem);
                }
                return oldItem;
            }
            return null;
        }

        //Unequip all items which are currently equipped.
        public void UnequipAll()
        {
            for(int i = 0; i < currentEquipment.Length; i++)
            {
                Unequip(i);
            }
            EquipDefaultItems();
        }

        void SetEquipmentBlendShapes(Equipment item, int weight)
        {
            foreach(EquipmentMeshRegion blendShape in item.coveredMeshRegions)
            {
                targetMesh.SetBlendShapeWeight((int) blendShape, weight);
            }
        }

        void Update()
        {
            if(Input.GetKey(KeyCode.U))
            {
                UnequipAll();
            }
        }
        //On game start, equip the default items
        void EquipDefaultItems()
        {
            foreach(Equipment item in defaultItems)
            {
                Equip(item);
            }
        }
    }
}


