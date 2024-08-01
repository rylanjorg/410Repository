using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    public enum ChestState
    {
        Closed,
        Opened
    }

    private bool interactable = false;
    private bool opening = false;
    private float holdTimer = 0f;
    private float requiredHoldTime = 1f; // Adjust the hold time as needed
    [SerializeField] float viewVectorDotProduct;
    [SerializeField] float dotProductThreshold = 0.5f;

    public int chestPrice;
    public TextMeshProUGUI uiText;
    public Slider progressBar; // Reference to the UI Slider

    public GameObject powerUpPrefab; // Assign your PowerUp prefab in the Unity Editor
    public GameObject damagePopUp;
    public Animator animator;
    private int  _openChestAnimID;

    public ChestState chestState;

    void Awake()
    {
        animator = GetComponent<Animator>();

        AssignAnimationIDs();
    }

    void Start()
    {
        // Disable the slider at the start
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }

        chestState = ChestState.Closed;
        //uiText.text = "";
    }

    void Update()
    {
        switch (chestState)
        {
            case ChestState.Closed:
                HandleClosedState();
                break;
            case ChestState.Opened:
                HandleOpenedState();
                break;
        }

     
    }

    void HandleClosedState()
    {
        ShowWSUI();
        if (interactable && !opening)
        {
           
            if (chestPrice > PlayerInfo.Instance.playerModifiers.currency)
            {
                uiText.text = "";
                // If the player doesn't have enough currency, disable UI and prevent chest opening
                //
                //progressBar.gameObject.SetActive(false);
            }
            else
            {
                
                uiText.text = "Hold [<color=#" + PlayerInfo.Instance.keybindColorRGB + ">F</color>] to open chest ($" + chestPrice + ")";
                //progressBar.gameObject.SetActive(false);
                if (Input.GetKey(KeyCode.F))
                {
                    if(progressBar == null) progressBar = PopUpGenerator.Instance.CreateInteractSliderPopUp().GetComponent<Slider>();
                    progressBar.gameObject.SetActive(true);
                    holdTimer += Time.deltaTime;
                    uiText.text = "Opening chest...";
                    if (holdTimer >= requiredHoldTime)
                    {
                        opening = true;
                        OpenChest();
                        if(progressBar != null) Destroy(progressBar.transform.gameObject, 1.0f);
                        //progressBar.gameObject.SetActive(false);
                    }
                }
                else
                {
                    ResetHoldTimer();
                }

                // Update the progress bar based on the holdTimer
                if (progressBar != null)
                {
                    progressBar.value = holdTimer / requiredHoldTime;
                }
            }
        }
        else
        {
            ResetHoldTimer();
        }
    }

    void HandleOpenedState()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(chestState == ChestState.Opened) return;
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("chest enter");
            uiText = PopUpGenerator.Instance.CreateInteractUIPopUp().GetComponent<TextMeshProUGUI>();
            interactable = true;
        }
        
       
        //damagePopUp.GetComponent<ChestPopUpAnimation>().popUpState = ChestPopUpAnimation.PopUpState.Active;
        //DamagePopUpGenerator.Instance.CreateChestPopUp(transform.position + new Vector3(0,2,0), $"$ {chestPrice.ToString()}", Color.white);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("chest exit");
            interactable = false;

            DestroyUIInstance();
            
            ResetHoldTimer();
        }
    }


    void ShowWSUI()
    {
        // Calculate the direction from the player to the chest
        Vector3 directionToChest = (transform.position - PlayerInfo.Instance.playerRuntimeDataList[0].generalData.playerRoot.transform.position).normalized;
        Debug.DrawRay(PlayerInfo.Instance.playerRuntimeDataList[0].generalData.playerRoot.transform.position, directionToChest, Color.magenta);


        // Calculate the dot product of the player's forward vector and the direction to the chest
        viewVectorDotProduct = Vector3.Dot(PlayerInfo.Instance.playerRuntimeDataList[0].generalData.mainCamera.transform.forward, directionToChest);

        // If the dot product is greater than 0.5, the player is facing the chest
        if (viewVectorDotProduct > dotProductThreshold)
        {
            // Show the UI
            if(damagePopUp == null) damagePopUp = PopUpGenerator.Instance.CreateChestPopUp(transform.position + new Vector3(0,2,0), $"$ {chestPrice.ToString()}", Color.white);
            
            
            if(damagePopUp != null && damagePopUp.GetComponent<ChestPopUpAnimation>().popUpState == ChestPopUpAnimation.PopUpState.InActive)
            {
                damagePopUp.GetComponent<ChestPopUpAnimation>().SetFadeIn();
            }
        }
        else
        {
            // Hide the UI
            if(damagePopUp != null && damagePopUp.GetComponent<ChestPopUpAnimation>().popUpState == ChestPopUpAnimation.PopUpState.Active)
            {
                damagePopUp.GetComponent<ChestPopUpAnimation>().SetFadeOut();
            }
        
        }
    }

    void OpenChest()
    {
        // Deduct the chest price from the player's currency
        PlayerInfo.Instance.playerModifiers.currency -= chestPrice;
        ResetHoldTimer();
        //uiText.text = "";

        animator.SetTrigger(_openChestAnimID);
        chestState = ChestState.Opened;
        DestroyUIInstance();

        PowerUpGenerator.Instance.CreatePowerUp(transform.position, Color.yellow);
        //GameObject newPowerUp = Instantiate(powerUpPrefab, transform.position, transform.rotation);
        //newPowerUp.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
        Destroy(damagePopUp, 0.5f);
        //Destroy(gameObject);
    }

    void AssignAnimationIDs()
    {
        // Assign the animation IDs
        _openChestAnimID = Animator.StringToHash("OpenChest");
    }


    void DestroyUIInstance()
    {
        if(uiText != null) 
        {
            Destroy(uiText.transform.gameObject, 1.0f);
            uiText = null;
        }
        if(progressBar != null) 
        {
            Destroy(progressBar.transform.gameObject, 1.0f);
            progressBar = null;
        }
    }

    void ResetHoldTimer()
    {
        opening = false;
        holdTimer = 0f;
    }
}
