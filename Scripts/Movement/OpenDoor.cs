using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] float openingSpeed;
    [SerializeField] Transform doorHinge;
    Animator animator;
    bool doorClosed;

    void Start()
    {
        animator = GetComponent<Animator>();
        //doorClosed = true;
    }
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            if (Input.GetKey(KeyCode.R))
            {
                Open();
            }
        }
    }

    void OnTriggerExit(Collider colex)
    {
        if(colex.tag == "Player")
        {
            Close();
        }
    }

    void Open()
    {
        animator.ResetTrigger("close");
        animator.SetTrigger("open");
    }

    void Close()
    {
        animator.ResetTrigger("open");
        animator.SetTrigger("close");
    }
}
