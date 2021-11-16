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
        if(Input.GetKey(KeyCode.R))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        Debug.Log("Picking up Sode");
        Destroy(this.gameObject, 1.5f);
    }
}
