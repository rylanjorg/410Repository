using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelShadePlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SkinnedMeshRenderer renderer;
    [SerializeField] Material[] materials;
    [SerializeField] float glowLerpTime = 1.0f;
    private Coroutine glowCoroutine;
    void Awake()
    {
        // Get the SkinnedMeshRenderer component from this GameObject
        renderer = GetComponent<SkinnedMeshRenderer>();

        // Get all the Materials from the Renderer
        materials = renderer.materials;
    }


    void Start()
    {
        //DisablePlayerGlow();
        //EnablePlayerGlow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePlayerGlow()
    {
        // If a glow coroutine is already running, stop it
        if (glowCoroutine != null)
        {
            StopCoroutine(glowCoroutine);
        }

        // Start a new glow coroutine and store a reference to it
        glowCoroutine = StartCoroutine(LerpShaderFloatPropertyOverTime("_Override_Color_Lerp", glowLerpTime));
    }

    public void EnablePlayerRenderer()
    {
        renderer.enabled = true;
    }

    public void DisablePlayerGlow()
    {
        // If a glow coroutine is running, stop it
        if (glowCoroutine != null)
        {
            StopCoroutine(glowCoroutine);
            glowCoroutine = null;
        }

        // Set the shader property to 0
        ModifyShaderFloatProperty("_Override_Color_Lerp", 0.0f);
    }

    public void DisablePlayerRenderer()
    {
        renderer.enabled = false;
    }


    public void ModifyShaderFloatProperty(string propertyName, float value)
    {
        // Iterate over each Material
        foreach (Material material in materials)
        {
            // Set the property on the material
            material.SetFloat(propertyName, value);
        }

        // Re-assign the modified materials array back to the renderer
        renderer.materials = materials;
    }

    public IEnumerator LerpShaderFloatPropertyOverTime(string propertyName, float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime <= duration)
        {
            float t = (Time.time - startTime) / duration;

            // Iterate over each Material
            foreach (Material material in materials)
            {
                // Lerp the property on the material
                material.SetFloat(propertyName, Mathf.Lerp(0, 1, t));
            }

            // Re-assign the modified materials array back to the renderer
            renderer.materials = materials;

            yield return null;
        }
    }
}
