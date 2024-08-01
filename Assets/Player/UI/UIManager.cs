using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    void Awake()
    {
        PlayerInfo.Instance.OnInitalizeEventSystems.AddListener(HandleInitialization);
    }

    void HandleInitialization()
    {
        //Debug.Log("UIManager Initialized by Initalization Event");
        AddListners();
    }

    void AddListners()
    {
        UIEventController.Instance.OnUpdateHealthUI.AddListener(UpdateHealthUI);
        UIEventController.Instance.OnUpdatePowerUpUI.AddListener(UpdatePowerUpUI);
        UIEventController.Instance.OnUpdateTextUI.AddListener(UpdateTextUI);
        UIEventController.Instance.OnCreateCanvasInstance.AddListener(CreateCanvasInstanceUI);
    }


    public void CreateCanvasInstanceUI(GameObject canvasPrefab, Action<GameObject> callback)
    {
        // Instantiate the canvas prefab
        GameObject instance = Instantiate(canvasPrefab);
        callback?.Invoke(instance);
    }
 

    public void UpdateHealthUI(int newValue)
    {
        //UpdateHealthUIHelper();
        Debug.Log("Health UI Updated , New Value: " + newValue);
    }

    public void UpdateTextUI(TextMeshProUGUI textUIElement, string newValue)
    {
        //UpdateHealthUIHelper();
        Debug.Log("Health UI Updated , New Value: " + newValue);
        textUIElement.text = newValue;
    }

    public void UpdatePowerUpUI(Sprite newSprite)
    {
        //ActivateNextImageSlot(newSprite);
    }

    

    // Function to activate the next available image slot and assign a power-up sprite
    public void ActivateNextImageSlot(Sprite powerUpSprite)
    {
        /*if (currentImageIndex < uiImageSlots.Length)
        {
            // Activate the next available image slot
            uiImageSlots[currentImageIndex].gameObject.SetActive(true);
            powerUpNumTexts[currentImageIndex].gameObject.SetActive(true);
            // Assign the power-up sprite to the activated image slot
            uiImageSlots[currentImageIndex].sprite = powerUpSprite;
            powerUpNumTexts[currentImageIndex].text = "x1";
            // Increment the index for the next activation
            currentImageIndex++;
        }
        else
        {
            Debug.LogWarning("No more image slots available.");
        }*/
    }
}
