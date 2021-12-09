﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RR.Inventories.Equipment
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
    public class Equipment : Item
    {
        public EquipmentSlot equipSlot;
        public override void Use()
        {
            base.Use();
            EquipmentManager.instance.Equip(this);
            RemoveFromInventory();
        }
    }

    public enum EquipmentSlot
    {
        Head,
        Chest,
        Legs,
        Weapon,
        Shield,
        Feet
    }
}


