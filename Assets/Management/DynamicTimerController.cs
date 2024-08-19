using System; // Add this line
using System.Collections.Generic;
using UnityEngine;

public class DynamicTimerController : MonoBehaviour
{
    public TimerManager timerManager;
    public Vector3 basePosition = new Vector3(100, 100, 0); // Base position for the first timer
    public float verticalSpacing = 50f; // Spacing between timers

    private List<Timer> activeTimers = new List<Timer>();

    public Timer CreateAndConfigureTimer(float duration, Sprite symbol, Action onTimerCompleted)
    {
        Vector3 newPosition = basePosition - new Vector3(0, activeTimers.Count * verticalSpacing, 0);

        Timer newTimer = timerManager.CreateTimer();

        RectTransform rectTransform = newTimer.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;

        newTimer.SetPosition(newPosition);
        newTimer.SetSymbol(symbol);
        newTimer.SetTime(duration);

        // Subscribe to the TimerCompleted event
        newTimer.TimerCompleted += onTimerCompleted;

        newTimer.StartTimer(duration);

        activeTimers.Add(newTimer);

        return newTimer;
    }

    void Update()
    {
        List<Timer> completedTimers = new List<Timer>();

        // Check for completed timers and mark them for removal
        foreach (var timer in activeTimers)
        {
            if (!timer.IsRunning)
            {
                completedTimers.Add(timer);
                Destroy(timer.gameObject); // Optional: destroy the timer's GameObject
            }
        }

        // Remove completed timers from the list
        foreach (var completedTimer in completedTimers)
        {
            activeTimers.Remove(completedTimer);
        }

        // Adjust the positions of remaining timers
        AdjustTimersPosition();
    }

    private void AdjustTimersPosition()
    {
        for (int i = 0; i < activeTimers.Count; i++)
        {
            Vector3 newPosition = basePosition - new Vector3(0, i * verticalSpacing, 0);
            activeTimers[i].SetPosition(newPosition);
        }
    }
}
