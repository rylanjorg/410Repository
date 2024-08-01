using UnityEngine;
using UnityEngine.VFX;

public class ParticleEmissionByDistance : MonoBehaviour
{
    public VisualEffect visualEffect;
    public float emissionRate = 10; // particles per unit of distance
    private Vector3 lastPosition;
    private float distanceTraveled;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        distanceTraveled += distance;

        float lerpFactor = Mathf.Clamp01(distanceTraveled);
        visualEffect.SetFloat("EmissionRate", Mathf.Lerp(0, emissionRate, lerpFactor));

        if (distanceTraveled > 1)
        {
            distanceTraveled = 0;
        }
    }
}