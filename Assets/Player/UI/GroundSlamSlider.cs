using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayerData;

namespace PlayerStates
{
    public class GroundSlamSlider : MonoBehaviour
    {
        public Slider slider; // Reference to the UI slider

        public void Update()
        {
            SetSliderValue();
        }
        
        public void SetSliderValue()
        {
            slider.value = Mathf.Clamp(PlayerInfo.Instance.playerRuntimeDataList[0].groundSlamDecisionRuntimeData.chargeAmount, slider.minValue, slider.maxValue);
        }
    }
}
