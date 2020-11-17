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
        Transform target;
        Mover _mover;
        
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
        }
        void Update()
        {
            if(target == null) return;//resume to normal movement
            
            //_mover.MoveTo(target.position);
            //If in range of target
            if (target != null && !RangeCheck())
            {
                _mover.MoveTo(target.position);
            }
            //If not close enough to target yet...
            else
            {
                _mover.Cancel();
            }
        }

        private bool RangeCheck()
        {
            return Vector3.Distance(target.position, transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            _actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }
        public void Cancel()
        {
            target = null;
        }
    }
}
