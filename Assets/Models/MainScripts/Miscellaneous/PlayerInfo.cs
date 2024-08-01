using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

using UnityEngine.Events;
using System;
using PlayerStates;
using PlayerData;
using System.Collections;
using System.Linq;



using PlayerData;


public class PlayerInfo : SerializedMonoBehaviour
{
    public enum PlayerInstanceMethod
    {
        Instantiate,
        Find
    }

    public UnityEvent OnInitalizeEventSystems;
    public UnityEvent OnInitalizePlayers;
    public static event Action OnPlayerInitialization;
    public static PlayerInfo Instance;

    // Player data
    
    [TabGroup("tab1/General/SubTabGroup","Player", TextColor = "orange")] [SerializeField] GameObject playerPrefab;
    [TabGroup("tab1/General/SubTabGroup","Player")] [SerializeField] public GameObject weaponObject;
    [TabGroup("tab1/General/SubTabGroup","Player")] [SerializeField] public PlayerModifiers playerModifiers;
    [TabGroup("tab1/General/SubTabGroup","Player")] [SerializeField] public List<PlayerModifiers> playersData;
    [TabGroup("tab1/General/SubTabGroup","Player")] [SerializeField] public List<PlayerRuntimeData> playerRuntimeDataList = new List<PlayerRuntimeData>();



    // UI data

    [TabGroup("tab1","General", TextColor = "green")]
    [TabGroup("tab1/General/SubTabGroup","UI", TextColor = "green")] [SerializeField] public GameObject PlayerCanvas;
    [TabGroup("tab1/General/SubTabGroup","UI", TextColor = "green")] [SerializeField] public GameObject CooldownSliderPrefab;
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] bool usePlayerHUD = true;
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public GameObject GameOverCanvas;
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] bool useGameOverCanvas = true;
    
    //[TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public Sprite[] powerUpSprites; // Array of power-up sprites
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public Image[] uiImageSlots; // Array of UI image slots
    //[TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public TextMeshProUGUI[] powerUpNumTexts; // Array of UI texts for number of each powerup
    
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public string currencyTextName; // Reference to the UI TextMeshPro component for currency
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public string powerUpTextName;   // Reference to the UI TextMeshPro component for power-ups
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public string healthTextName; // Reference to the UI TextMeshPro component for hp
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public string frontHealthBarName;
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public string backHealthBarName;
    [TabGroup("tab1/General/SubTabGroup","UI")] [SerializeField] public string interactIconName;
    [TabGroup("tab1/General/SubTabGroup","UI")] [DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "RuntimeData")] [SerializeField] public Dictionary<string, PowerUpRuntimeData> powerUpRuntimeDataDict = new Dictionary<string, PowerUpRuntimeData>();








  


    

    [TabGroup("tab1","Dynamic", TextColor = "blue")]
    [TabGroup("tab1/Dynamic/SubTabGroup","UI", TextColor = "green")] [ReadOnly] public GameObject playerCanvasInstance;
    [TabGroup("tab1/Dynamic/SubTabGroup","UI", TextColor = "green")] [ReadOnly] public GameObject gameOverCanvasInstance;
    [TabGroup("tab1/Dynamic/SubTabGroup","UI", TextColor = "green")] [ReadOnly] public GameObject cooldownSliderInstance;
    [TabGroup("tab1/Dynamic/SubTabGroup","Player", TextColor = "orange")] [ReadOnly] public List<GameObject> playerInstances;
    [TabGroup("tab1/Dynamic/SubTabGroup","UI")] [SerializeField] [ReadOnly] public TextMeshProUGUI currencyText; // Reference to the UI TextMeshPro component for currency
    [TabGroup("tab1/Dynamic/SubTabGroup","UI")] [SerializeField] [ReadOnly] public TextMeshProUGUI powerUpText;   // Reference to the UI TextMeshPro component for power-ups
    [TabGroup("tab1/Dynamic/SubTabGroup","UI")] [SerializeField] [ReadOnly] public TextMeshProUGUI healthText; // Reference to the UI TextMeshPro component for hp
    [TabGroup("tab1/Dynamic/SubTabGroup","UI")] [SerializeField] [ReadOnly] public Image frontHealthBar;
    [TabGroup("tab1/Dynamic/SubTabGroup","UI")] [SerializeField] [ReadOnly] public Image backHealthBar;
    [TabGroup("tab1/Dynamic/SubTabGroup","UI")] [SerializeField] [ReadOnly] public Image interactIcon;



    [TabGroup("tab1/Dynamic/SubTabGroup","UI")] int currentImageIndex = 0;

    
    
    [TabGroup("tab1/Dynamic/SubTabGroup","Player", TextColor = "orange")] GameObject playerInstance;




    [TabGroup("tab1","Game State", TextColor = "purple")]
    [TabGroup("tab1","Game State")] [SerializeField] bool gameOver = false;
    [TabGroup("tab1","Game State")] [SerializeField] [Range(1,5)] int numberOfPlayers = 1;
    [TabGroup("tab1","Game State")] [SerializeField] PlayerInstanceMethod playerInstanceMethod = PlayerInstanceMethod.Instantiate;
    


    /*[TabGroup("tab1","Inscribed", TextColor = "green")]
    [TabGroup("tab1/Inscribed/SubTabGroup", "General", TextColor = "lightgreen")] [SerializeField] private CharacterController characterController;
    [TabGroup("tab1/Inscribed/SubTabGroup", "General")] [SerializeField] private GameObject characterMovementRoot;
    
    [Title("Targeting:")]
    [TabGroup("tab1/Inscribed/SubTabGroup", "General")] [SerializeField] public GameObject characterTargetRoot;
    [TabGroup("tab1/Inscribed/SubTabGroup", "General")] [SerializeField] public TargetMode targetMode;

    [Title("References:")]
    [TabGroup("tab1/Inscribed/SubTabGroup", "General")] [SerializeField] public ProceduralLegController proceduralLegController;


    [TabGroup("tab1/Inscribed/SubTabGroup", "States", TextColor = "purple")] [SerializeField]  public List<StateWrapper> stateWrappers = new List<StateWrapper>{  };
    [TabGroup("tab1/Inscribed/SubTabGroup", "States")] [SerializeField]  public StateWrapper initialState;
    
    

    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [ReadOnly] [SerializeReference] public State currentStateInstance;
    [TabGroup("tab1", "Dynamic")] [ReadOnly] public string currentStateName;
    [TabGroup("tab1", "Dynamic")] [ReadOnly] [SerializeField] private GameObject currentTarget { get; }
    [TabGroup("tab1", "Dynamic")] [ReadOnly] [SerializeField] public State initialStateInstance;
    [TabGroup("tab1", "Dynamic")] [SerializeReference] public List<State> stateInstances = new List<State> {};
    [TabGroup("tab1", "Dynamic")] [ReadOnly] [ShowIf("targetMode", TargetMode.PlayerOnly)] [SerializeField] public GameObject playerTargetRoot;*/






    private float lerpTimer;
    private bool checkForBackToMenu = false;




    public List<PowerUp> powerUpList = new List<PowerUp>();


    public WalkActionData walkActionData;


    [HideInInspector]
    public Stopwatch timer;

    public Color keybindColor;
    public Color nanoRejuvenatorColor;
    public Color scavengerMagnetColor;
    public Color havocSerumColor;
    public Color shieldMatrixColor;
    public Color sonicAcceleratorColor;

    [HideInInspector]
    public string keybindColorRGB;
    [HideInInspector]
    public string nanoRejuvenatorRGB;
    [HideInInspector]
    public string scavengerMagnetRGB;
    [HideInInspector]
    public string havocSerumRGB;
    [HideInInspector]
    public string shieldMatrixRGB;
    [HideInInspector]
    public string sonicAcceleratorRGB;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            OnInitalizeEventSystems = new UnityEvent();
            OnInitalizePlayers = new UnityEvent();

            OnInitalizeEventSystems.AddListener(HandlePlayerInitialization);
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

    public void InitalizePlayers()
    {
        OnInitalizePlayers.Invoke();
    }



    void HandlePlayerInitialization()
    {
        //Debug.Log("VFX Spawner Initialized by Initalization Event");
        //AddListners();

        if(playerInstanceMethod == PlayerInstanceMethod.Instantiate)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerInstances.Add(Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity));
            }
            SetPlayerRuntimeData();
        }
        else if(playerInstanceMethod == PlayerInstanceMethod.Find)
        {
            SetPlayerRuntimeData();
        }

        
    }
    
    
    
    void Start()
    {
        InitalizeEventSystems();
        InitalizePlayers();

        OnStartCanvas();
      
        playerModifiers.health = playerModifiers.maxHealth;
        
        SetRGB();
        
        timer = this.GetComponent<Stopwatch>();
        timer.StartStopwatch();

        UpdateUI();

        if (playerCanvasInstance != null)
            playerCanvasInstance.SetActive(true);

        if (gameOverCanvasInstance != null)
            gameOverCanvasInstance.SetActive(false);
            
        OnPlayerInitialization?.Invoke();

        if(usePlayerHUD && playerCanvasInstance != null) 
        {
            playerCanvasInstance.SetActive(true);
        }
        else if(!usePlayerHUD && playerCanvasInstance != null)
        {
            playerCanvasInstance.SetActive(false);
        }
    }


    // Player Main Canvas
    // ----------------------------------------------

    void OnStartCanvas()
    {
        if(UIEventController.Instance != null)  UIEventController.Instance.CreateCanvasInstance(PlayerCanvas, (GameObject instance) => { 
            playerCanvasInstance = instance; 
            AssignCanvasReferences();
        });

        // Set all UI powerup images and texts to false at the start
        foreach (Image uiImage in uiImageSlots)
        {
            if(uiImage.gameObject != null) uiImage.gameObject.SetActive(false);
        }

        //interactIcon.gameObject.SetActive(false);

        if(usePlayerHUD) playerCanvasInstance.SetActive(true);
        else playerCanvasInstance.SetActive(false);
    }

    void AssignCanvasReferences()
    {
        if(playerCanvasInstance != null)
        {
            frontHealthBar = playerCanvasInstance.transform.Find(frontHealthBarName).GetComponent<Image>();
            backHealthBar = playerCanvasInstance.transform.Find(backHealthBarName).GetComponent<Image>();
            currencyText = playerCanvasInstance.transform.Find(currencyTextName).GetComponent<TextMeshProUGUI>();
            powerUpText = playerCanvasInstance.transform.Find(powerUpTextName).GetComponent<TextMeshProUGUI>();
            healthText = playerCanvasInstance.transform.Find(healthTextName).GetComponent<TextMeshProUGUI>();
            //interactIcon = playerCanvasInstance.transform.Find(interactIconName).GetComponent<Image>();
        }

       
        for (int i = 0; i < uiImageSlots.Length; i++)
        {
            string iconName = "PowerUp Icon " + (i + 1);
            uiImageSlots[i] = FindTransformInHierarchy(playerCanvasInstance.transform, iconName).GetComponent<Image>();
        }
    }

    // ----------------------------------------------

    void SetRGB()
    {
        keybindColorRGB = ColorUtility.ToHtmlStringRGB(keybindColor);
        nanoRejuvenatorRGB = ColorUtility.ToHtmlStringRGB(nanoRejuvenatorColor);
        scavengerMagnetRGB = ColorUtility.ToHtmlStringRGB(scavengerMagnetColor);
        havocSerumRGB = ColorUtility.ToHtmlStringRGB(havocSerumColor);
        shieldMatrixRGB = ColorUtility.ToHtmlStringRGB(shieldMatrixColor);
        sonicAcceleratorRGB = ColorUtility.ToHtmlStringRGB(sonicAcceleratorColor);
    }


    void Update()
    {
        

        //UpdateUI();


        if(playerCanvasInstance != null)
        {
            if (playerModifiers.health <= 0f && gameOver == false)
            {
                Destroy(playerPrefab);
                Destroy(weaponObject);
                gameOver = true;
                playerCanvasInstance.SetActive(false);
                Time.timeScale = 0.1f;
                Invoke("GameOverUI", 0.25f);
            }
            if (Input.GetKeyDown(KeyCode.Space) && checkForBackToMenu)
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

     // Method to add a power-up and specify the amount
    public void AddPowerUp(string powerUpType, int amount, PowerUpData powerUpData)
    {
        if (!powerUpRuntimeDataDict.ContainsKey(powerUpType))
        {
            
            //PowerUpData powerUpData = powerUps[powerUpType].powerUpData;
            PowerUpRuntimeData newPowerUpRuntimeData = new PowerUpRuntimeData(powerUpData);
            powerUpRuntimeDataDict.Add(powerUpType, newPowerUpRuntimeData);
            newPowerUpRuntimeData.UpdatePowerUpText();
        }
        else
        {
            powerUpRuntimeDataDict[powerUpType].amount += amount;
        }

        if (powerUpType == "Nano-Rejuvenator")
        {
            playerModifiers.maxHealth = playerModifiers.maxHealth + 10f;
            playerModifiers.health = playerModifiers.health + 10f;
        }
        else if (powerUpType == "Sonic Accelerator")
        {
            playerModifiers.playerSpeed = playerModifiers.playerSpeed + 1.0f;
            //walkActionData.setPlayerSpeed(playerModifiers.playerSpeed);
        }
        else if (powerUpType == "Havoc Serum")
        {
            playerModifiers.attackSpeed += 0.25f;
        } 
        else if (powerUpType == "Shield Matrix")
        {
            playerModifiers.defense = playerModifiers.defense + 0.05f;
        }
        else if (powerUpType == "Scavenger Magnet")
        {
            playerModifiers.lootingMultiplier = playerModifiers.lootingMultiplier + 0.2f;
        }

        AssignPowerUpUISlots();

        // Update UI after adding a power-up
        UpdateUI();
    }

    

    // Method to update the UI
    public void UpdateUI()
    {
        // need to come back and make these so that it only runs when needed so like when the playuer takes damage rather than every update.
        UpdateHealthUI();
        UpdateCurrencyText();
        foreach (PowerUpRuntimeData powerUp in powerUpRuntimeDataDict.Values)
        {
            powerUp.UpdatePowerUpText();
        }
        UIEventController.Instance.UpdateHealthUI(0);
    }

    void GameOverUI()
    {
        if(useGameOverCanvas)
        {
            if(UIEventController.Instance != null)  UIEventController.Instance.CreateCanvasInstance(GameOverCanvas, (GameObject instance) => { gameOverCanvasInstance = instance; });
            else Debug.LogError("UIEventController.Instance is null");
    
            gameOverCanvasInstance.SetActive(true);
        }
    
        Time.timeScale = 0f;
        checkForBackToMenu = true;
    }


    // Method to update the currency text
    void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            string newCurrencyText = "$" + playerModifiers.currency.ToString();
            UIEventController.Instance.UpdateTextUI(currencyText, newCurrencyText);
        }
    }

    void UpdateHealthUI()
    {
        playerModifiers.health = Mathf.Clamp(playerModifiers.health, 0, playerModifiers.maxHealth);
        UIEventController.Instance.UpdateTextUI(healthText, playerModifiers.health.ToString() + " / " + playerModifiers.maxHealth.ToString());
    }


    private void AssignPowerUpUISlots()
    {
        // Ensure we have the same number of runtime data entries and UI slots
        if (powerUpRuntimeDataDict.Count > uiImageSlots.Count())
        {
            Debug.LogError("Mismatch between number of runtime data entries and UI slots");
            return;
        }

        // Get the keys from the dictionary
        var keys = new List<string>(powerUpRuntimeDataDict.Keys);

        // Assign each runtime data entry to a UI slot
        for (int i = 0; i < keys.Count; i++)
        {
            // Get the runtime data for this key
            var runtimeData = powerUpRuntimeDataDict[keys[i]];

            // Assign the runtime data to the UI slot
            //iSlots[i].AssignRuntimeData(runtimeData);
            AssignUISlot(i, keys);
        }
    }


    private void AssignUISlot(int i, List<string> keys)
    {
        Debug.Log("Assigning UI Slot " + i + " to " + keys[i] + " with sprite " + powerUpRuntimeDataDict[keys[i]].powerUpData.powerUpSprite.name);
        uiImageSlots[i].sprite = powerUpRuntimeDataDict[keys[i]].powerUpData.powerUpSprite;
        uiImageSlots[i].gameObject.SetActive(true);
        powerUpRuntimeDataDict[keys[i]].assignedUIImageSlot = uiImageSlots[i];
        powerUpRuntimeDataDict[keys[i]].powerUpText = uiImageSlots[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        powerUpRuntimeDataDict[keys[i]].UpdatePowerUpText();

        
    }


    public void TakeDamage(float damage)
    {
        playerModifiers.health -= Mathf.Ceil(playerModifiers.damage * (1f - playerModifiers.defense)); // round defended damage up to nearest whole number
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        playerModifiers.health += healAmount;
        lerpTimer = 0f;
    }

    public void UpdateHealthUIHelper()
    {
        if (frontHealthBar == null || backHealthBar == null || healthText == null)
        {
            Debug.Log("One or more UI elements are not assigned.");
            return;
        }

        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = playerModifiers.health / playerModifiers.maxHealth;

        if (healthText != null)
        {
            string healthTextString = playerModifiers.health.ToString() + " / " + playerModifiers.maxHealth.ToString();
            UIEventController.Instance.UpdateTextUI(healthText, healthTextString);
        }

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / playerModifiers.chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.color = Color.blue;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / playerModifiers.chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    

    public void CreateUISlider(PlayerRuntimeData data)
    {
        if (cooldownSliderInstance == null)
        {
            cooldownSliderInstance = GameObject.Instantiate(CooldownSliderPrefab, playerCanvasInstance.transform);
            cooldownSliderInstance.transform.SetParent(playerCanvasInstance.transform, false);

           // Start the coroutine to destroy the slider when the state changes
            StartCoroutine(DestroySliderWhenStateChanges(data));
        }
    }

    private IEnumerator DestroySliderWhenStateChanges(PlayerRuntimeData data)
    {  
        MovementState initialState = data.currentState;
        // Wait until the current state is no longer the initial state
        while (data.currentState == initialState)
        {
            yield return null; // Wait for the next frame
        }

        
        // Destroy the cooldownSliderInstance
        if (cooldownSliderInstance != null)
        {
            Destroy(cooldownSliderInstance);
            cooldownSliderInstance = null;
        }     
    }

    private void SetPlayerRuntimeData()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerIdentifier");
        foreach (GameObject player in players)
        {
            playerInstances.Add(player);
            GameObject playerRootContainer = FindTransformInHierarchy(player.transform, "PlayerMovementManager").gameObject;
            if (playerRootContainer != null)
            {
                PlayerRuntimeData playerRuntimeData = playerRootContainer.GetComponent<PlayerDataManagement>().playerRuntimeData;
                if(playerRuntimeData != null)
                {
                    playerRuntimeDataList.Add(playerRuntimeData);
                }
            }
        }
    }

    public static Transform FindTransformInHierarchy(Transform parent, string name)
    {
        // Check if the current transform's name matches the target name
        if (parent.name == name)
        {
            return parent;
        }

        // Check all children
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform result = FindTransformInHierarchy(parent.GetChild(i), name);

            if (result != null)
            {
                return result;
            }
        }

        // If the transform was not found in this hierarchy, return null
        return null;
    }


}
