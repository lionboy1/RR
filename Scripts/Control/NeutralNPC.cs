using UnityEngine;
using RR.Control;

namespace RR.Control
{
    //Attach this to a neutral npc in place of the 'AIController' script
    
    public class NeutralNPC : AIController
    {
        public override bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Check if aggravation timer has expired
            return  timeSinceAggravated < _aggroCooldownTime || distanceToPlayer < chaseDistance;
        }

        public override void AggravateNearbyEnemies()
        {
            
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);//No direction for sphere, it surounds the GameObject.
            foreach(RaycastHit hit in hits)
            {
                AIController enemy = hit.transform.GetComponent<AIController>();
                CharacterClasses npcClass = hit.transform.GetComponent<CharacterClasses>();
                if(enemy == null) continue;
                //else
                if( (int)npcClass.nPC == (int)this.GetComponent<CharacterClasses>().nPC)
                {
                    enemy.Aggravate();
                }
            }
        }
    }
}