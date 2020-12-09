using UnityEngine;

namespace RR.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController _animatorOverride = null;
        [SerializeField] GameObject equippedPrefab;
        [SerializeField] float weaponRange = 2.0f;
        [SerializeField] float _weaponDamage;
        public void Spawn(Transform handTransform, Animator anim)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform);
            }
            if(_animatorOverride != null)
            {
                anim.runtimeAnimatorController = _animatorOverride;
            }
        }
        public float GetRange()
        {
            return weaponRange;
        }
        public float GetDamage()
        {
            return _weaponDamage;
        }
    }
}