using UnityEngine;
using RR.Control;

namespace RR.Control
{
    //Attach this to a aggressive npc in place of the 'AIController' script
    
    public class AggressiveNPC : AIController
    {
        public override bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Check if aggravation timer has expired
            return distanceToPlayer < chaseDistance || timeSinceAggravated <_aggroCooldownTime;
        }
    }
}