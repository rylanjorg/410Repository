using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

using UnityEngine.Events;
using System;

  

public class SettingsUICanvasManager : MonoBehaviour
{
    public UnityEvent OnInitalizeEventSystems;
    public static SettingsUICanvasManager Instance;

    [TabGroup("tab1","General", TextColor = "green")]
    [TabGroup("tab1/General/SubTabGroup","UI", TextColor = "green")] [SerializeField] public GameObject PlayerSettingsCanvas;
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] bool useSettingsCanvas = true;
    

    [TabGroup("tab1","Dynamic", TextColor = "blue")] [ReadOnly] public GameObject playerSettingsCanvasInstance;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            OnInitalizeEventSystems = new UnityEvent();

            UIEventController.Instance.OnOpenCloseSettingsUIEvent += HandleSettingsUIOpenCloseEvent;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void InitalizeEventSystems()
    {
        OnInitalizeEventSystems.Invoke();
    }

    public void HandleSettingsUIOpenCloseEvent()
    {
        //Debug.LogError("HandleSettingsUIOpenCloseEvent event invoked recieved");
        if(playerSettingsCanvasInstance.activeSelf)
        {
            playerSettingsCanvasInstance.SetActive(false);
        }
        else
        {
            playerSettingsCanvasInstance.SetActive(true);
        }
    }



    void Start()
    {
        InitalizeEventSystems();

     
        if(UIEventController.Instance != null)  UIEventController.Instance.CreateCanvasInstance(PlayerSettingsCanvas, (GameObject instance) => { playerSettingsCanvasInstance = instance; });
        else Debug.LogError("UIEventController.Instance is null");
        
        playerSettingsCanvasInstance.SetActive(false);

        /*
        if(useSettingsCanvas) 
        {
            playerSettingsCanvasInstance.SetActive(true);
        }
        else
        {
            playerSettingsCanvasInstance.SetActive(false);
        }*/

       
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
           UIEventController.Instance.InvokeOpenCloseSettingsUIEvent();
        }
    }

}
