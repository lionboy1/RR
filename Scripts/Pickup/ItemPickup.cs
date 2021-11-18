using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    bool inTriggerZone;
    int keyPress = 0;

    void Start()
    {
        inTriggerZone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && inTriggerZone)
        {
            keyPress++;
            if(keyPress == 1)
            {
                Pickup();
            }
            return;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            inTriggerZone = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            inTriggerZone = false;
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
