using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TutorialTrigger : MonoBehaviour
{
    public string tutorialText;
    public TextMeshProUGUI uiText;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("enter");
            uiText.text = tutorialText; // Set the TextMeshPro text to the tutorialText value
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("exit");
            uiText.text = ""; // Set the TextMeshPro text to blank
            Destroy(gameObject);
        }
    }
}
