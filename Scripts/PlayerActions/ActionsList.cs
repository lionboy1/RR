using UnityEngine;

public class ActionsList : MonoBehaviour
{
    Animator _playerAnim;
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
        Trap
    }

    //Access PlayerActions enum
    public PlayerActions playerActions;

    void Start()
    {
       _playerAnim = GameObject.Find("Player").GetComponent<Animator>();
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
                switch(playerActions)
                {
                    case PlayerActions.Fish:
                        _playerAnim.SetTrigger("fish");
                        break;
                    case PlayerActions.Cook:
                        _playerAnim.SetTrigger("cook");
                        break;
                    case PlayerActions.Build:
                        _playerAnim.SetTrigger("build");
                        break;
                    case PlayerActions.Fire:
                        _playerAnim.SetTrigger("fire");
                        break;
                    case PlayerActions.Forage:
                        _playerAnim.SetTrigger("forage");
                        break;
                    case PlayerActions.Forge:
                        _playerAnim.SetTrigger("forge");
                        break;
                    case PlayerActions.Talk:
                        _playerAnim.SetTrigger("talk");
                        break;
                    case PlayerActions.Trap:
                        _playerAnim.SetTrigger("trap");
                        break;
                    case PlayerActions.Rest:
                        _playerAnim.SetTrigger("rest");
                        break;
                    case PlayerActions.Smash:
                        _playerAnim.SetTrigger("smash");
                        break;    
                }
            }
        }    
    }   
}
