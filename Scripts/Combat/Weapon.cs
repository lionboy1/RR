using RR.Core;
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
        [SerializeField] bool isRightHanded;
        [SerializeField] Projectile projectile = null;

        public void Spawn(Transform rightHand, Transform leftHand,Animator anim)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Instantiate(equippedPrefab, handTransform);
            }
            if (_animatorOverride != null)
            {
                anim.runtimeAnimatorController = _animatorOverride;
            }
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            //Will raycast and let physics handle projectile flight instead of setting target
            //projectileInstance.SetTarget(target);
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