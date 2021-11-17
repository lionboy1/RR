using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        Debug.Log("Picking up " + item.name);
        //Was there space in the inventory to add item?
        bool wasPickedUp = Inventory.instance.Add(item);
        
        if(wasPickedUp)
        {
            Destroy(this.gameObject, 1.5f);
        }

        
    }
}
