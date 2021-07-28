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

        //Used for determining if item can be dropped
        public static bool isEquipped = false;

        public void Spawn(Transform rightHand, Transform leftHand,Animator anim)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Instantiate(equippedPrefab, handTransform);
                isEquipped = true;
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
            
            //Later, use raycast and let physics handle projectile flight instead of setting target
            projectileInstance.SetTarget(target, _weaponDamage);
        }


        public float GetRange()
        {
            return weaponRange;
        }
        public float GetDamage()
        {
            return _weaponDamage;
        }

        
        //This will be called by the new weapon being picked up
        void Drop()
        {
            if(isEquipped)
            {
                //Drop only is player picks up another weapon.
                //Spawn a prefab without pickup script
                //Enable rigidbody to allow it to fall
                
                ///OntriggerExit will then spawn the real pickup 
                //in its place when the player leaves trigger
            }
        }
    }
}