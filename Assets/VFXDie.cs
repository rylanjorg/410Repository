using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXDie : MonoBehaviour
{
    float delay = 0.1f;
    private float time = 0.0f;
    // Start is called before the first frame update

    void Update()
    {
        time += Time.deltaTime;
        int aliveCount = GetComponentInChildren<VisualEffect>().aliveParticleCount;

        if (aliveCount == 0 && time > delay)
        {
            Destroy(this.gameObject);
        }

    }

}
