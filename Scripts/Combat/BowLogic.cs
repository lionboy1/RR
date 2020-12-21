using UnityEngine;

namespace RR.Combat
{
    public class BowLogic : MonoBehaviour 
    {
        [SerializeField] GameObject projectilePrefab = null;
        [SerializeField] Transform projectileSpawnPos;


        ///// Example similar to Sharp Accent https://www.youtube.com/watch?v=gARh1de-obM

        void Start()
        {
            //Instantiate projectile prefab with the projectile script attached to it.
            projectilePrefab = Instantiate(projectilePrefab, projectileSpawnPos);
        }
        
    }
}