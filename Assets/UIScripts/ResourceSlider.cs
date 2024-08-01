using TMPro;
using UnityEngine;
using UnityEngine.UI;

using PlayerData;
using PlayerStates;

public class ResourceSlider : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public PlayerRuntimeData playerRuntimeData;
    public SlideActionData slideActionData;
    public TextMeshProUGUI mText;

    void Start()
    {
        // Ensure that the slider is assigned in the Inspector
        if (slider == null)
        {
            Debug.LogError("Slider component is not assigned.");
            enabled = false; // Disable the script to prevent errors
            return;
        }

        // Set the maximum value of the slider
        //slider.maxValue = resourceData.maximumResource;

        // Set the initial value of the slider
        //slider.value = resourceData.GetCurrentResource();
    }

    // Function to update the meter's value
    public void Update()
    {
        /*slider.value = Mathf.Clamp(resourceData.GetCurrentResource(), 0f, resourceData.maximumResource);
        mText.text = "Slide Resource Meter: \nSlideActionResourceCost: " + slideActionData.slideResourceCost + "\n" +
            "CurrentResource: " + resourceData.GetCurrentResource() + "\n" +
            "MaxResource: " + resourceData.maximumResource;

        */
    }
}
