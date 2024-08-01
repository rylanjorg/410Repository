using UnityEngine;
using UnityEngine.VFX;

public class KillVFX : MonoBehaviour
{
    public VisualEffect vfx;
    public float killTime = 5f;

    private float timer;

    void Update()
    {
        // Check if there are any alive particles
        if (vfx.aliveParticleCount > 0)
        {
            // Reset the timer if there are alive particles
            timer = 0f;
        }
        else
        {
            // Increment the timer if there are no alive particles
            timer += Time.deltaTime;

            // Destroy the GameObject if the timer exceeds the kill time
            if (timer > killTime)
            {
                Destroy(gameObject);
            }
        }
    }
}