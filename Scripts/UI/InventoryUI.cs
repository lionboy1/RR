﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    //Inventory panel parent to search for children
    public Transform itemsParent;
    InventorySlot[] slots;
    Inventory inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        //Subscribe to the inventory delegate for inventory changes
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}