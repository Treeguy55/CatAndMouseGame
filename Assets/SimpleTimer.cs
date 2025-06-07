using UnityEngine;
using TMPro;

public class SimpleTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = "Time: " + elapsedTime.ToString("F1") + "s";
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f; // reset when starting fresh
    }

    public void PauseTimer()
    {
        isRunning = false;
    }
}
