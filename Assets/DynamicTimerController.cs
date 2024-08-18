using System.Collections.Generic;
using UnityEngine;

public class DynamicTimerController : MonoBehaviour
{
    public TimerManager timerManager;
    public Vector3 basePosition = new Vector3(100, 100, 0); // Base position for the first timer
    public float verticalSpacing = 50f; // Spacing between timers

    private List<Timer> activeTimers = new List<Timer>();

    public Timer CreateAndConfigureTimer(float duration, Sprite symbol)
    {
        // Calculate the position based on the number of active timers
        Vector3 newPosition =
            basePosition - new Vector3(0, activeTimers.Count * verticalSpacing, 0);

        // Create the new timer using the TimerManager
        Timer newTimer = timerManager.CreateTimer();
        newTimer.SetTime(duration);
        newTimer.SetSymbol(symbol);
        newTimer.SetPosition(newPosition);
        newTimer.StartTimer(duration);

        // Add the new timer to the list of active timers
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
            }
        }

        // Remove completed timers from the list
        foreach (var completedTimer in completedTimers)
        {
            activeTimers.Remove(completedTimer);
            Destroy(completedTimer.gameObject); // Optional: destroy the timer's GameObject
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
