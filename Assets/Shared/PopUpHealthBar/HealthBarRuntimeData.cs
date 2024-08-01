using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerData
{
    [System.Serializable]
    public class HealthBarRuntimeData 
    {
        HealthBarData healthBarData;

        public HealthBarRuntimeData(HealthBarData healthBarData)
        {
            this.healthBarData = healthBarData;
        }

        
    }
}
