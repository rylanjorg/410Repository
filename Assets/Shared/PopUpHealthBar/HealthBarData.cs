using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerData
{
    [CreateAssetMenu(menuName = "ScriptableObject/PopUpHealthBar/HealthBarData")]
    public class HealthBarData : ScriptableObject
    {
        float targetHeight = 1.0f;
        float targetScale = 1.0f;
        float lifetime = 5.0f;
        public AnimationCurve opacityCurve;
        public AnimationCurve scaleCurve;
        public AnimationCurve heightCurve;  
    }
}
