using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

namespace PlayerData
{
    [System.Serializable]
    public class PowerUpRuntimeData 
    {
        public PowerUpData powerUpData;
        public int amount;
        public Image assignedUIImageSlot;
        public TextMeshProUGUI powerUpText; 
        

        public PowerUpRuntimeData(PowerUpData powerUpData)
        {
            this.powerUpData = powerUpData;
            //uiImageSlot = PlayerInfo.Instance.FindTransformInHierarchy(powerUpData.uiImageSlotName).GetComponent<Image>();
            amount = 1;
            
        }

        // Method to update the power-up text
        public void UpdatePowerUpText()
        {
            if (powerUpText != null)
            {
                //string text = "<size=50><u>Power-Ups:</u></size>\n";
                string text = "";
                UIEventController.Instance.UpdateTextUI(powerUpText, text);
                //powerUpText.text += $"{powerUpData.name} (x{amount})\n";
                powerUpText.text += $"(x{amount})\n";
            }
        }

        // Method to get the amount of a specific power-up
        public int GetPowerUpAmount(string powerUpType)
        {
            return amount;
        }
    }
        
       
}
