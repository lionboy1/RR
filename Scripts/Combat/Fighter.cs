using UnityEngine;
using RR.Movement;
using RR.Core;
using System.Collections;

namespace RR.Combat
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction 
    {
       
        [SerializeField]
        float timeBetweenAttacks = 1.0f;
        float timeSinceLastAttack = Mathf.Infinity;//Starts as if already happened, so character can do the first attack immediately.
        Health target;
        Mover _mover;
        Animator _anim;
        
        [SerializeField] Weapon defaultWeapon = null;//Scriptable Object
        
        
        ActionScheduler _actionScheduler;
        bool _canAttack = false;
        
        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;
        Weapon _currentWeapon = null;
        
        #region
        void Awake() 
        {
            //yield return new WaitForSeconds(5);
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
                //if (!animator) Debug.Log($"{name} has no Animator");
            }
        }
        #endregion
        #region
        void Start()
        {
            EquipWeapon(defaultWeapon);
        }
        #endregion
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;//resume to normal movement
            if(target.IsDead()) return;
            //If in range of target
            if (target != null && !RangeCheck())
            {
                _mover.MoveTo(target.transform.position, 1f);//The maximum value (100%) for full speed.
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
            return Vector3.Distance(target.transform.position, transform.position) < _currentWeapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) return false;
            Health targetToCheck = combatTarget.GetComponent<Health>();
            //Now that the combat target is found, it is not null
            //However, still check to see if it is dead or alive
            return targetToCheck && !targetToCheck.IsDead();//Is IsDead is actually true, then it won't proceed to attack target.
        }

        //Since the playe and AI share the fighter class, the player cannot
        //have a CombatTarget component, so the GameObject argument is used instead
        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
            target.GetComponent<Health>().Damage(0f);
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            _anim.ResetTrigger("attack");//Get it ready to be used next time
            _anim.SetTrigger("stopAttack");
        }

        void AttackBehavior()
        {
            transform.LookAt(target.transform);
            //Throttle attacks
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
                //target.Damage(_currentWeapon.GetDamage());
            }
        }

        private void TriggerAttack()
        {
            _anim.ResetTrigger("stopAttack");//resets trigger
            _anim.SetTrigger("attack");
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, _anim);
            _anim.SetTrigger("Pickup");
            Invoke("StopPickup", 2);
            
        }

        void StopPickup()
        {
            _anim.SetTrigger("StopPickup");
        }
        //Animation event
        public void Hit()
        {
            if(target == null) return;

            if(_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            //Handle damage later - maybe via raycast from weapon instead
            /*else
            {
                target.Damage(_currentWeapon.GetDamage());
            }*/
        }
        //Animation event
        /*public void ResetPickupTrigger()
        {
            _anim.ResetTrigger("Pickup");
        }*/
        
    }
}
