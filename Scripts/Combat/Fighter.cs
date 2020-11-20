using UnityEngine;
using RR.Movement;
using RR.Core;

namespace RR.Combat
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction {

        [SerializeField]
        float weaponRange = 2.0f;
        [SerializeField]
        float timeBetweenAttacks = 1.0f;
        float timeSinceLastAttack = 0;
        Health target;
        Mover _mover;
        Animator _anim;
        [SerializeField]
        float _weaponDamage;
        
        ActionScheduler _actionScheduler;
        
        void Start() 
        {
            _mover = GetComponent<Mover>();
            if(_mover == null)
            {
                Debug.LogError("Mover component not found");
            }

            _actionScheduler = GetComponent<ActionScheduler>();
            if(_actionScheduler == null)
            {
                Debug.LogError("ActionScheduler component not found");
            }
            _anim = GetComponent<Animator>();
            if( _anim == null)
            {
                Debug.LogError("Animator component not attached!");
            }
        }
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;//resume to normal movement
            if(target.IsDead()) return;
            //If in range of target
            if (target != null && !RangeCheck())
            {
                _mover.MoveTo(target.transform.position);
            }
            //If not close enough to target yet...
            else
            {
                _mover.Cancel();
                //Add animator code here for attack animation
                AttackBehavior();
            }
        }

        private bool RangeCheck()
        {
            return Vector3.Distance(target.transform.position, transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            _actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
            target.GetComponent<Health>().Damage(3f);
        }
        public void Cancel()
        {
            target = null;
        }
        void AttackBehavior()
        {
            transform.LookAt(target.transform);
            //Throttle attacks
            if(timeSinceLastAttack > timeBetweenAttacks)    
            {   
                _anim.SetTrigger("attack");
                timeSinceLastAttack = 0;
                target.Damage(_weaponDamage);
            }
        }
    }
}
