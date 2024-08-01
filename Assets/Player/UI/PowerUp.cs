using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace PlayerData
{
    [System.Serializable]
    public class PowerUp 
    {
        public string name;
        public int amount;
        public Sprite powerUpSprite;
        public TextMeshProUGUI powerUpNumTexts;
        public TextMeshProUGUI powerUpText; 
    }
}

