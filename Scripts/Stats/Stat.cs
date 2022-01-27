using System.Collections.Generic;
using System.Collections;
using UnityEngine;
//Brackeys - WIP

namespace RR.Stats
{
    //Since this class is not deriving from monobehaviour
    //serialize it to make it visible in the inspector
    [System.Serializable]

    public class Stats
    {
        int baseValue;
        public int GetValue()
        {
            return baseValue;
        }
    }
}
