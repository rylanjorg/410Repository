using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

[RequireComponent(typeof(UIManager))]
public class UIEventController : MonoBehaviour
{
    // Define a new UnityEvent that takes an int parameter
    [System.Serializable]
    public class UpdateHealthUIEvent : UnityEvent<int> { }
    public class UpdatePowerUpUIEvent : UnityEvent<Sprite> { }
    public class UpdateTextUIEvent : UnityEvent<TextMeshProUGUI, string> { }
    public class CreateCanvasInstanceEvent : UnityEvent<GameObject, Action<GameObject>> { }


    // Create an instance of the event
    public UpdateHealthUIEvent OnUpdateHealthUI;
    public UpdatePowerUpUIEvent OnUpdatePowerUpUI;
    public UpdateTextUIEvent OnUpdateTextUI;
    public CreateCanvasInstanceEvent OnCreateCanvasInstance;

    
    // An event that will be invoked when the settings menu is opened or closed:
    public delegate void OpenCloseSettingsUIEvent();
    public event OpenCloseSettingsUIEvent OnOpenCloseSettingsUIEvent;


    public UIManager uiManager;
    // Singleton instance
    public static UIEventController Instance { get; private set; }
  
    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        OnUpdateHealthUI = new UpdateHealthUIEvent();
        OnUpdatePowerUpUI = new UpdatePowerUpUIEvent();
        OnUpdateTextUI = new UpdateTextUIEvent();
        OnCreateCanvasInstance = new CreateCanvasInstanceEvent();

        uiManager = GetComponent<UIManager>();
    }

    public void UpdateHealthUI(int newValue)
    {
        OnUpdateHealthUI.Invoke(newValue);
    }
    public void UpdatePowerUpUI(Sprite newSprite)
    {
        OnUpdatePowerUpUI.Invoke(newSprite);
    }
    public void CreateCanvasInstance(GameObject canvasPrefab, Action<GameObject> callback)
    {
        OnCreateCanvasInstance.Invoke(canvasPrefab, callback);
    }
    public void UpdateTextUI(TextMeshProUGUI textUIElement, string newValue)
    {
        OnUpdateTextUI.Invoke(textUIElement, newValue);
    }

    public void InvokeOpenCloseSettingsUIEvent()
    {
        OnOpenCloseSettingsUIEvent?.Invoke();
    }


}
