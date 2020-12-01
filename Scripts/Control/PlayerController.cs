using UnityEngine;
using RR.Movement;
using RR.Combat;
using RR.Core;

namespace RR.Control
{
    [RequireComponent(typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        Fighter _fighter;
        Health _health;
        Mover _mover;

        void Start()
        {
            _fighter = GetComponent<Fighter>();
            if(_fighter == null)
            {
                Debug.LogError("Fighter Component not found!");
            }
            _health = GetComponent<Health>();
            if(_health == null)
            {
                Debug.LogError("Health component not found");
            }
            _mover = GetComponent<Mover>();
            if(_mover == null)
            {
                Debug.LogError("Mover component not found");
            }
        }
        void Update()
        {
            if(_health.IsDead()) return;
            if(InteractWithCombat()) return;//If clicked is enemy, attack.
            if(InteractWithMovement()) return;
        }

        bool InteractWithMovement()
    
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if(Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point, 1f);//1f just means do max value since max is set to 1 in the range.  The player will go faster based on animation anyways.
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            //Check to see what was hit
            foreach( RaycastHit hit in hits)
            {
                //Does object that is hit have the CombatTarget component?
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;//Skip this current iteration only and move to the next iteration

                if(!_fighter.CanAttack(target.gameObject)) continue;//abort this current iteration and continue loop.
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }
    }
}
