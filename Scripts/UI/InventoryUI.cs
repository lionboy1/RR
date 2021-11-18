using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    //Inventory panel parent to search for children
    public Transform itemsParent;
    InventorySlot[] slots;
    Inventory inventory;
    public GameObject inventoryUI;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        //Subscribe to the inventory delegate for inventory changes
        inventory.onItemChangedCallback += UpdateUI;

        //slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            //Hide UI
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
    void UpdateUI()
    {
        //This allows the subscribers to talk with the delegate, set to true
        //So UI will update even when it's not visible to player.
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>(true);
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
