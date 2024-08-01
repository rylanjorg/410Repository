using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using PlayerData;

public class GroundSlamCooldownUI : MonoBehaviour
{
    public Image slideIcon;
    public Image imageCooldown;
    public Image numChargesBackground;
    public TMP_Text textCooldown;
    public TMP_Text textNumCharges;
    //public RechargableResourceData resourceData;
    public PlayerRuntimeData playerRuntimeData;
    public Image keybindFrame;
    public Image numChargesFrame;


    void Awake()
    {
        PlayerInfo.OnPlayerInitialization +=  SetPlayerData;    
    }
    
    void Start()
    {
        
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
        numChargesBackground.fillAmount = 1.0f;
       

    }

    void SetPlayerData()
    {
        playerRuntimeData = PlayerInfo.Instance.playerRuntimeDataList[0];
    }

    void Update()
    {
        
       
        if (playerRuntimeData.GroundSlam_resourceRuntimeData.GetCurrentResource() < 0.5) // 0 charges
        {
            textNumCharges.text = "0";
            slideIcon.color = new Color(1f, 1f, 1f, 0.3f);
            keybindFrame.color = new Color(1f, 1f, 1f, 0.3f);
            numChargesFrame.color = new Color(1f, 1f, 1f, 0.3f);
            textCooldown.gameObject.SetActive(true);
            imageCooldown.fillAmount = 1f - (playerRuntimeData.GroundSlam_resourceRuntimeData.GetCurrentResource() / 0.5f);
            numChargesBackground.fillAmount = (playerRuntimeData.GroundSlam_resourceRuntimeData.GetCurrentResource() / 0.5f);
            if (playerRuntimeData.GroundSlam_resourceRuntimeData.GetCurrentResource() < 0.17f)
            {
                textCooldown.text = "3";
            }
            else if (playerRuntimeData.GroundSlam_resourceRuntimeData.GetCurrentResource() < 0.33f)
            {
                textCooldown.text = "2";
            }
            else
            {
                textCooldown.text = "1";
            }
        }
        else if (playerRuntimeData.GroundSlam_resourceRuntimeData.GetCurrentResource() < 1)
        {
            textNumCharges.text = "1";
            slideIcon.color = new Color(1f, 1f, 1f, 0.8f);
            keybindFrame.color = new Color(1f, 1f, 1f, 0.8f);
            numChargesFrame.color = new Color(1f, 1f, 1f, 0.8f);
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0;
            numChargesBackground.fillAmount = (playerRuntimeData.GroundSlam_resourceRuntimeData.GetCurrentResource() - 0.5f) / 0.5f;
        }
        else
        {
            textNumCharges.text = "2";
            slideIcon.color = new Color(1f, 1f, 1f, 0.8f);
            keybindFrame.color = new Color(1f, 1f, 1f, 0.8f);
            numChargesFrame.color = new Color(1f, 1f, 1f, 0.8f);
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0;
            numChargesBackground.fillAmount = 1f;
        }
        
    }
}
