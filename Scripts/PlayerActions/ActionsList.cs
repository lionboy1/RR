using UnityEngine;
using System.Collections;
using System;

public class ActionsList : MonoBehaviour
{
    Animator _playerAnim;
    Destructibles destructibles;
    int keyStrokes = 0;
    public enum PlayerActions
    {
        Fish,
        Cook,
        Fire,
        Build,
        Forage,
        Forge,
        Talk,
        Smash,
        Rest,
        Trap,
        Kick,
        PickUp,
        OpenChest,
        OpenDoor
    }

    //Access PlayerActions enum
    public PlayerActions playerActions;

    void Start()
    {
       _playerAnim = GameObject.Find("Player").GetComponent<Animator>();
       destructibles = GetComponent<Destructibles>();
    }
    // Update is called once per frame
    void Update()
    {
            
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {    
            if(Input.GetKeyDown(KeyCode.R))
            {
                keyStrokes++;
                if(keyStrokes ==1)
                {
                    switch(playerActions)
                    {
                        case PlayerActions.Fish:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("fish");
                            break;
                        case PlayerActions.Cook:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("cook");
                            break;
                        case PlayerActions.Build:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("build");
                            break;
                        case PlayerActions.Fire:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("fire");
                            break;
                        case PlayerActions.Forage:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("forage");
                            break;
                        case PlayerActions.Forge:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("forge");
                            break;
                        case PlayerActions.Talk:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("talk");
                            break;
                        case PlayerActions.Trap:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("trap");
                            break;
                        case PlayerActions.Rest:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("rest");
                            break;
                        case PlayerActions.Smash:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("smash");
                            destructibles.DestroyObject();
                            break;
                        case PlayerActions.Kick:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("kick");
                            //Invoke("CallDestructibleFun", 0.2f);
                            StartCoroutine(CallDestructibleFun());
                            break;
                        case PlayerActions.PickUp:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("pickup");
                            break;
                        case PlayerActions.OpenChest:
                            other.transform.LookAt(this.gameObject.transform);
                            _playerAnim.SetTrigger("openchest");
                            break;             
                    }
                }    
            }
        }    
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            keyStrokes = 0;
        }
    }
    IEnumerator CallDestructibleFun()   
    {
        yield return new WaitForSeconds(0.5f);
        destructibles.DestroyObject();
    }
}
