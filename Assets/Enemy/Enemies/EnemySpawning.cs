using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    public List<Vector3> spawnPointList = new List<Vector3>();
    private int randEnemy;
    private int randPosition;

    // Start is called before the first frame update
    void Start()
    {
        // while there are still enemies to spawn and spawn points to use
        while (enemyList.Count > 0 && spawnPointList.Count > 0)
        {
            // sets randEnemy and randPosition to random numbers
            randEnemy = Random.Range(0, enemyList.Count);
            randPosition = Random.Range(0, spawnPointList.Count);
            // sets tempSpawnPoint to the value in spawnPointList at randPosition
            Vector3 tempSpawnPoint = spawnPointList[randPosition];
            // if the enemy is a FlyingEnemy, adds between 3 and 6 to the spawn height
            if (enemyList[randEnemy].name == "FlyingEnemy")
            {
                tempSpawnPoint.y = Random.Range(3, 6);
            }
            // Instantiate the enemy at the modified spawn point
            Instantiate(enemyList[randEnemy], tempSpawnPoint, Quaternion.identity);
            // Remove the instantiated enemy and its spawn point from the lists
            enemyList.RemoveAt(randEnemy);
            spawnPointList.RemoveAt(randPosition);
        }
    }
}
