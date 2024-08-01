using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
{
    public TextMeshProUGUI stopwatchText; // Reference to the TextMeshProUGUI object
    public TextMeshProUGUI gameOverText;

    [HideInInspector]
    private float elapsedTime = 0f;
    private bool isRunning = false;

    public float difficultyIncrease = 0.1f;
    public float difficultyTimeInterval = 120f;

    

    private void Start()
    {
        UpdateStopwatchText();
        Invoke("ApplyDifficultyLevel", 1f);
    }

    private void ApplyDifficultyLevel()
    {
        int difficultyLevel = PlayerPrefs.GetInt("DifficultyLevel", 0);
        for (float i = 0; i < difficultyLevel; i += difficultyIncrease)
        {
            RaiseDifficulty();
            Debug.Log("difficulty raised");
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateStopwatchText();

            // Check if the elapsed time is a multiple of difficultyTimeInterval
            if (Mathf.FloorToInt(elapsedTime / difficultyTimeInterval) > Mathf.FloorToInt((elapsedTime - Time.deltaTime) / difficultyTimeInterval))
            {
                // Call the RaiseDifficulty function
                RaiseDifficulty();
            }
        }
        
    }

    public void StartStopwatch()
    {
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        elapsedTime = 0f;
        UpdateStopwatchText();
    }

    private void RaiseDifficulty()
    {
        // Find all GameObjects with the specified tag
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");

        // Iterate through each GameObject and call your function
        foreach (GameObject obj in gameObjectsWithTag)
        {
            // Check if the object has the script you want to call a function on
            EnemyInfo enemyInfo = obj.GetComponent<EnemyInfo>();

            if (enemyInfo != null)
            {
                enemyInfo.maxHealth += enemyInfo.baseHealth * 0.1f;
            }
        }
    }

    private void UpdateStopwatchText()
    {   
        if (stopwatchText != null)
        {
            stopwatchText.text = FormatTime(elapsedTime);
        }
        if (gameOverText != null)
        {
            gameOverText.text = "You survived for " + FormatTime(elapsedTime) + " (Level 0)";
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
