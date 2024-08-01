using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpGenerator : MonoBehaviour
{
    public static PopUpGenerator Instance;
    public float ammoSpacing = 200f;
    public GameObject damageCanvasPrefab;
    public GameObject chestCanvasPrefab;
    public GameObject interactCanvasPrefab;
    public GameObject attackCanvasPrefab;
    public GameObject interactSliderCanvasPrefab;



    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CreatePopUp(Vector3.zero, Random.Range(0, 100).ToString(), Color.yellow);
        }
    }

    public void CreatePopUp(Vector3 position, string text, Color color, AudioClip audioClip = null)
    {
        var damagePopUp = Instantiate(damageCanvasPrefab, position, Quaternion.identity);
        Transform tempTrans = damagePopUp.transform;
        var temp = damagePopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;
        
        
        if(audioClip != null)
        {
            AudioSource audioSource = tempTrans.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }

        Destroy(damagePopUp, 1.0f);
    }

    public GameObject CreateChestPopUp(Vector3 position, string text, Color color, AudioClip audioClip = null)
    {
        var damagePopUp = Instantiate(chestCanvasPrefab, position, Quaternion.identity);
        Transform tempTrans = damagePopUp.transform;
        var temp = damagePopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;
        
        
        if(audioClip != null)
        {
            AudioSource audioSource = tempTrans.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }

        return damagePopUp;
        //Destroy(damagePopUp, 1.0f);
    }

    public GameObject CreateInteractUIPopUp()
    {
        var popUp = Instantiate(interactCanvasPrefab);
        // Set the parent of the popUp
        popUp.transform.SetParent(PlayerInfo.Instance.playerCanvasInstance.transform, false);
        var temp = popUp.transform.GetChild(0).GetComponent<Image>();
        temp.gameObject.SetActive(false);


        Transform tempTrans = popUp.transform;
        //var temp = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //temp.text = text;
        //temp.faceColor = color;
        
        
        /*if(audioClip != null)
        {
            AudioSource audioSource = tempTrans.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }*/

        return popUp;
    }

    public GameObject CreateAttackUIPopUp(int attackID)
    {
         var popUp = Instantiate(attackCanvasPrefab);
        // Set the parent of the popUp
        popUp.transform.SetParent(PlayerInfo.Instance.playerCanvasInstance.transform, false);
        var temp = popUp.transform.GetChild(0).GetComponent<Image>();
        temp.gameObject.SetActive(false);

        // Calculate the offset based on the attackID
        float offset = ammoSpacing * attackID;

        // Apply the offset to the position of the popUp
        Vector3 position = popUp.transform.localPosition;
        position.x += offset;
        popUp.transform.localPosition = position;

        return popUp;
    }

    public GameObject CreateInteractSliderPopUp()
    {
        var popUp = Instantiate(interactSliderCanvasPrefab);
        // Set the parent of the popUp

        //Vector3 originalLocalPosition = popUp.transform.localPosition;
        popUp.transform.SetParent(PlayerInfo.Instance.playerCanvasInstance.transform, false);
        //popUp.transform.localPosition = originalLocalPosition;
        //var temp = popUp.transform.GetChild(0).GetComponent<Image>();
        //temp.gameObject.SetActive(false);


        Transform tempTrans = popUp.transform;
        //var temp = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //temp.text = text;
        //temp.faceColor = color;
        
        
        /*if(audioClip != null)
        {
            AudioSource audioSource = tempTrans.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }*/

        return popUp;
    }
}
