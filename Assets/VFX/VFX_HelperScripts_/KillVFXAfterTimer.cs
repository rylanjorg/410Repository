using UnityEngine;

public class KillVFXAfterTimer : MonoBehaviour
{
    public float lifeTime = 5f; // The lifetime of the VFX in seconds

    // Start is called before the first frame update
    void Start()
    {
        // Destroy the GameObject after the specified lifetime
        Destroy(gameObject, lifeTime);
    }
}