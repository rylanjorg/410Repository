using UnityEngine;

public class ParticleMaterialLerp : MonoBehaviour
{
    public new ParticleSystem particleSystem = new ParticleSystem();
    public Material particleMaterial;
    public string propertyName;
    public float startValue;
    public float endValue;

    private Material instanceMaterial;
    private float lifetime;
    private float timer;

    void Start()
    {
        var main = particleSystem.main;
        lifetime = main.startLifetime.constant;
        timer = 0f;

        // Create an instance of the material
        instanceMaterial = new Material(particleMaterial);

        // Assign the instance material to the Particle System
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.material = instanceMaterial;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / lifetime);
        float value = Mathf.Lerp(startValue, endValue, t);

        // Modify the instance material
        instanceMaterial.SetFloat(propertyName, value);
    }
}