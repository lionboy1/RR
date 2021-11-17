using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region
    //Singleton to always access this inventory instance
    public static Inventory instance;

    //Initialize the instance

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More than one inventory instance found");
            return;
        }
        instance = this;
    }
    #endregion
    
    //Inventory UI will need to know when to update
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    
    public List<Item> items = new List<Item>();
    //How many inventory slots are available to populate
    public int space = 20;

    public bool Add(Item item)
    {
        if(!item.isDefaultItem)
        {
            //Is there enough space to add pickup?
            if(items.Count >= space)
            {
                Debug.Log("Not enough room");
                return false;
            }
            items.Add(item);
            //Send message to subscribers when item added
            if(onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        //If there was space and item added, return true, add item to inventory and delete from scene via itemPickup.cs
        return true;
    }
    
    public void Remove(Item item)
    {
        items.Remove(item);
        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
