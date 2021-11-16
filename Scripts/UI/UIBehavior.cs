using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehavior : MonoBehaviour
{
    bool inventoryVisible;
    [SerializeField] GameObject inv;
    
    // Start is called before the first frame update
    void Start()
    {
        inv.SetActive(false);
        inventoryVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        ShowHide();
    }

    void ShowHide()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!inventoryVisible)
            {
                inv.SetActive(true);
                inventoryVisible = true;
            }
            else if(inventoryVisible)
            {
                inv.SetActive(false);
                inventoryVisible = false;
            }
        }
    }
}
