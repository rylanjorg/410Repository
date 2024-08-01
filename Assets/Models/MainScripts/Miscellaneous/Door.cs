using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public TextMeshProUGUI interactTextbox; // UI TextMeshPro object
    public string nextSceneName; // Name of the next scene
    public GameObject boss; // GameObject that must not exist for the door to open (boss)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactTextbox != null && boss == null)
            {
                interactTextbox.text = "Press [<color=#E1E551>F</color>] to enter the vault";
            }
            else if (interactTextbox != null)
            {
                interactTextbox.text = "Kill the <color=#FF0000>guard</color> in order to enter the vault";
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F) && boss == null)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Clear the message when the player exits the trigger
            if (interactTextbox != null)
            {
                interactTextbox.text = "";
            }
        }
    }
}