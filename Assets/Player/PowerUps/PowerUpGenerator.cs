using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerUpGenerator : MonoBehaviour
{
    public static PowerUpGenerator Instance;
    public List<GameObject> powerUpPrefabs;
    public AudioClip spawnSound;
    public float initalYVelocity = 10;



    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
           CreatePowerUp(Vector3.zero, Color.yellow);
        }
    }

    public GameObject GetPowerUp()
    {
        int randomIndex = Random.Range(0, powerUpPrefabs.Count);
        GameObject powerUp = powerUpPrefabs[randomIndex];
        return powerUp;
    }

    public void AddVelocity(GameObject powerUp)
    {
        Rigidbody rb = powerUp.GetComponent<Rigidbody>();
        if(rb != null)
        {
            // Add upward velocity
            Vector3 velocity = new Vector3(0, initalYVelocity, 0);

            // Add random horizontal velocity
            float angle = Random.Range(0, 2 * Mathf.PI); // Random angle in radians
            float horizontalSpeed = 2; // Adjust this value to change the horizontal speed
            velocity += new Vector3(Mathf.Cos(angle) * horizontalSpeed, 0, Mathf.Sin(angle) * horizontalSpeed);

            rb.velocity = velocity;
        }
    }
        
    public void CreatePowerUp(Vector3 position, Color color, AudioClip audioClip = null)
    {
        var damagePopUp = Instantiate(GetPowerUp(), position, Quaternion.identity);
        //AddVelocity(damagePopUp);
        //Transform tempTrans = damagePopUp.transform;
        //var temp = damagePopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //temp.text = text;
        //temp.faceColor = color;
        
        
        /*if(audioClip != null)
        {
            AudioSource audioSource = tempTrans.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(spawnSound);
            }
        }*/

        //Destroy(damagePopUp, 1.0f);
    }
    /*
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
        var popUp = Instantiate(interactCanvasPrefab, Vector3.zero, Quaternion.identity);
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

        /*return popUp;
    }

    public GameObject CreateInteractSliderPopUp()
    {
        var popUp = Instantiate(interactSliderCanvasPrefab, Vector3.zero, Quaternion.identity);
        // Set the parent of the popUp
        popUp.transform.SetParent(PlayerInfo.Instance.playerCanvasInstance.transform, false);
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

       // return popUp;
    //}
}

