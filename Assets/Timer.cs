using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Image symbolImage; // Reference to the Image component for the symbol

    private float timeRemaining;
    private bool timerIsRunning = false;

    // Public property to check if the timer is running
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
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetSymbol(Sprite symbol)
    {
        if (symbolImage != null)
        {
            symbolImage.sprite = symbol;
            symbolImage.enabled = symbol != null; // Only enable the image if a symbol is set
        }
    }

    public void SetTime(float duration)
    {
        timeRemaining = duration;
        DisplayTime(timeRemaining);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
