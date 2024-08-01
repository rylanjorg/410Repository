using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerData
{
    [CreateAssetMenu(menuName = "ScriptableObject/FSM/RechargableResource")]
    public class RechargableResourceData : ScriptableObject
    {
        [Header("Inscribed")]

        public float resourceRechargeRate = 0.1f;
        public float resourceRechargeDelay = 0.1f;
        public float resourceDepletionRate = 0.1f;
        public float maximumResource = 1.0f;
    }
}