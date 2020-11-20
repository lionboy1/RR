using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RR.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        float _health = 100f;
        Animator _anim;
        bool isDead = false;

        //Getter so other classes can query is target is dead or not
        public bool IsDead()
        {
            return isDead;
        }

        void Start()
        {
            _anim = this.GetComponent<Animator>();
            if(_anim == null)
            {
                Debug.LogError("Animator not found");
            }
        }

        void Update()
        {

        }

        public void Damage(float damage)
        {
            _health = Mathf.Max(_health -= damage, 0);//Reduces health but not under 0
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
        }
    }
}

