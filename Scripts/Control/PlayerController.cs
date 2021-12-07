using UnityEngine;
using RR.Movement;
using RR.Combat;
using RR.Core;
using System;

namespace RR.Control
{
    [RequireComponent(typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        Fighter _fighter;
        Health _health;
        Mover _mover;

        enum CursorType
        {
            None,
            Movement,
            Combat
        }

       
        [System.Serializable]
          //Variable to hold data for cursor types
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        //Array of the struct above
        [SerializeField] CursorMapping[] cursorMappings = null;
       
 
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
            if(_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if(InteractWithCombat()) return;//If clicked is enemy, attack.
            if(InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        bool InteractWithMovement()
    
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, Mathf.Infinity);

            if (hasHit && !hit.collider.GetComponent<Blocked>())
            {
                if(Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point, 1f);//1f just means do max value since max is set to 1 in the range.  The player will go faster based on animation anyways.
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            //Check to see what was hit
            foreach( RaycastHit hit in hits)
            {
                //Does object that is hit have the CombatTarget component?
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;//Skip this current iteration only and move to the next iteration

                if(!_fighter.CanAttack(target.gameObject)) continue;//abort this current iteration and continue loop.
                if(Input.GetMouseButtonDown(0))
                {
                    _fighter.Attack(target.gameObject);
                    target.gameObject.GetComponent<AIController>().Aggravate();
                }
                SetCursor(CursorType.Combat);
                return true;
            }
            return false;
        }

        void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
        CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
    }
}
