using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Image symbolImage; // This needs to be assigned in the prefab

    private float timeRemaining;
    private bool timerIsRunning = false;

    public event Action TimerCompleted; // Event triggered when the timer completes

    public bool IsRunning
    {
        get { return timerIsRunning; }
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        timerIsRunning = true;
        DisplayTime(timeRemaining);
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining); // Ensure the last update is shown
                Debug.Log("Timer completed. Invoking TimerCompleted event.");
                TimerCompleted?.Invoke(); // Trigger the event when the timer completes
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.ForceMeshUpdate(); // Force update to ensure text is refreshed
    }

    public void SetSymbol(Sprite symbol)
    {
        if (symbolImage != null)
        {
            symbolImage.sprite = symbol;
            symbolImage.SetNativeSize();
            symbolImage.enabled = symbol != null;
        }
        else
        {
            Debug.LogError("Symbol Image is not assigned in the Timer prefab.");
        }
    }

    public Sprite GetJobSymbol()
    {
        return symbolImage.sprite;
    }

    public void SetTime(float duration)
    {
        timeRemaining = duration;
        DisplayTime(timeRemaining);
    }

    public void SetPosition(Vector3 position)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
    }
}
