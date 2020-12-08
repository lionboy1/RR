using UnityEngine;

namespace RR.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        float _health = 100f;
        Animator _anim;
        ActionScheduler _actionScheduler;
        bool isDead = false;

        //Getter so other classes can query is target is dead or not
        public bool IsDead()
        {
            return isDead;
        }

        void Awake()
        {
            _anim = this.GetComponent<Animator>();
            if(_anim == null)
            {
                Debug.LogError("Animator not found");
            }
            _actionScheduler = GetComponent<ActionScheduler>();
            if(_actionScheduler == null)
            {
                Debug.LogError("actionScheduler component not found!");
            }
        }

     
        public void Damage(float damage)
        {
            _health = Mathf.Max(_health - damage, 0);//Reduces health but not under 0
            if(_health == 0)
            {
               Die();
            }
            
        }
        void Die()
        {
            if (isDead) return;//back out of the function is isDead is already true;
                
            //Otherwise, continue
            _anim.SetTrigger("die");
            isDead = true;

            _actionScheduler.CancelCurrentAction();
        }
    }
}

