using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public Sprite[] powerUpSprites; // Array of power-up sprites
    public Image[] uiImageSlots; // Array of UI image slots

    private int currentImageIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Set all UI images to false at the start
        foreach (Image uiImage in uiImageSlots)
        {
            uiImage.gameObject.SetActive(false);
        }
    }

    // Function to activate the next available image slot and assign a power-up sprite
    public void ActivateNextImageSlot(Sprite powerUpSprite)
    {
        if (currentImageIndex < uiImageSlots.Length)
        {
            // Activate the next available image slot
            uiImageSlots[currentImageIndex].gameObject.SetActive(true);

            // Assign the power-up sprite to the activated image slot
            uiImageSlots[currentImageIndex].sprite = powerUpSprite;

            // Increment the index for the next activation
            currentImageIndex++;
        }
        else
        {
            Debug.LogWarning("No more image slots available.");
        }
    }
}
