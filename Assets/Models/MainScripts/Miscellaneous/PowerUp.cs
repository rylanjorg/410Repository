using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using PlayerData;

public class PowerUp : MonoBehaviour
{
    public string powerUpTitle = "Power-Up Title";
    public string powerUpDescription = "Power-Up Description";

    public TextMeshProUGUI uiText;
    public PowerUpData powerUpData;

    private bool playerInRange = false;
    public string powerUpColor;

    void Start()
    {
        //uiText = GameObject.FindGameObjectWithTag("InteractTextBox").GetComponent<TextMeshProUGUI>();
        //

       
        // Randomly generate type of powerup
        /*float randomValue = Random.value; // Random.value generates a random float between 0.0 and 1.0
        if (randomValue < 0.2f)
        {
            //Debug.Log("Attack Boost");
            powerUpTitle = "Havoc Serum";
            powerUpDescription = "Increases attack speed by 25%";
            powerUpColor = PlayerInfo.Instance.havocSerumRGB;
        }
        else if (randomValue < 0.4f)
        {
            //Debug.Log("Health Boost");
            powerUpTitle = "Nano-Rejuvenator";
            powerUpDescription = "Increases Max HP by 10";
            powerUpColor = PlayerInfo.Instance.nanoRejuvenatorRGB;
        }
        else if (randomValue < 0.6f)
        {
            //Debug.Log("Looting Boost");
            powerUpTitle = "Scavenger Magnet";
            powerUpDescription = "Increases enemy drops by 20%";
            powerUpColor = PlayerInfo.Instance.scavengerMagnetRGB;
        }
        else if (randomValue < 0.8f)
        {
            //Debug.Log("Speed Boost");
            powerUpTitle = "Sonic Accelerator";
            powerUpDescription = "Increases movement speed by 15%";
            powerUpColor = PlayerInfo.Instance.sonicAcceleratorRGB;
        }
        else
        {
            //Debug.Log("Defense Boost");
            powerUpTitle = "Shield Matrix";
            powerUpDescription = "Reduces damage of incoming attacks by 5%";
            powerUpColor = PlayerInfo.Instance.shieldMatrixRGB;
        }*/

    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PickUpPowerUp();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;

            if(uiText == null) uiText = PopUpGenerator.Instance.CreateInteractUIPopUp().GetComponent<TextMeshProUGUI>();

            uiText.text = "Press [<color=#" + PlayerInfo.Instance.keybindColorRGB + ">F</color>] to pick up <color=#" + powerUpColor + ">" + powerUpTitle + "</color>\n" + "<size=36>" + powerUpDescription + "</size>";
            /*for (int i = 0; i < PlayerInfo.Instance.powerUpSprites.Length; i++)
            {
                if (powerUpTitle == PlayerInfo.Instance.powerUpSprites[i].name)
                {
                    PlayerInfo.Instance.interactIcon.sprite = PlayerInfo.Instance.powerUpSprites[i];
                    PlayerInfo.Instance.interactIcon.gameObject.SetActive(true);
                }
            }*/
        }
    }

    
    void DestroyUIInstance()
    {
        if(uiText != null) 
        {
            Destroy(uiText.transform.gameObject, 1.0f);
            uiText = null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        
            DestroyUIInstance();
            uiText = null;
            if(PlayerInfo.Instance.interactIcon != null) PlayerInfo.Instance.interactIcon.gameObject.SetActive(false);
        }
    }

    void PickUpPowerUp()
    {
        // Implement power-up effects here
        //Debug.Log("Picked up " + powerUpTitle + "!");
        PlayerInfo.Instance.AddPowerUp(powerUpTitle, 1, powerUpData);
        //uiText.text = "";
        DestroyUIInstance();
        //PlayerInfo.Instance.interactIcon.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
