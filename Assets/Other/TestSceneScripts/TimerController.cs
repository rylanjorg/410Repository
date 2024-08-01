using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    private bool isTimerRunning = false;

    private void Start()
    {
        startTime = Time.time;
        isTimerRunning = true;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            float elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);
        }
    }

    private void UpdateTimerText(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}
