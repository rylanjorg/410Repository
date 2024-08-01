using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace PlayerData 
{
    [CreateAssetMenu(menuName = "ScriptableObject/UI/PowerUpData")]
    public class PowerUpData : ScriptableObject
    {
        public string name;
        public Sprite powerUpSprite;
    }
}

