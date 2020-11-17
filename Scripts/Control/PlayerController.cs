using UnityEngine;
using RR.Movement;
using RR.Combat;

namespace RR.Control
{
    public class PlayerController : MonoBehaviour
    {
        void Update()
        {
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
                    GetComponent<Mover>().StartMoveAction(hit.point);
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

                if(Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
                }
                return true;
            }
            return false;
        }
    }
}
