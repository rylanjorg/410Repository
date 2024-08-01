using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;

public class SpawnParticlesInCircle : MonoBehaviour
{
    public GameObject particlePrefab;
    public int raycastCount = 10;
    public float radius = 5f;
    public float raycastDistance = 10f;
    public LayerMask raycastLayerMask;
    public float spawnInterval = 0.1f;

    void Start()
    {
        StartCoroutine(SpawnParticles());
    }

    IEnumerator SpawnParticles()
    {
        for (int i = 0; i < raycastCount; i++)
        {
            float angle = i * 360f / raycastCount;
            float randomRadius = Random.Range(0, radius);
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 start = transform.position + direction * randomRadius;

            RaycastHit hit;
            if (Physics.Raycast(start, Vector3.down, out hit, raycastDistance, raycastLayerMask))
            {
                // Instantiate a new particle system at the hit point
                GameObject newParticle = Instantiate(particlePrefab, hit.point, Quaternion.identity);
                newParticle.GetComponent<ParticleSystem>().Play();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}