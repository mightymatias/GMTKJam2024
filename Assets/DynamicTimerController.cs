using UnityEngine;
using System.Collections.Generic;

public class DynamicTimerController : MonoBehaviour
{
    public TimerManager timerManager;
    public Vector3 basePosition = new Vector3(100, 100, 0); // Base position for the first timer
    public float verticalSpacing = 50f; // Spacing between timers

    private List<Timer> activeTimers = new List<Timer>();

    // Method to create and configure a timer with dynamic time, symbol, and position
    public void CreateAndConfigureTimer(float duration, Sprite symbol)
    {
        // Calculate the position for the new timer based on the number of active timers
        Vector3 newPosition = basePosition + new Vector3(0, -activeTimers.Count * verticalSpacing, 0);

        // Create the new timer using the TimerManager
        Timer newTimer = timerManager.CreateTimer();
        newTimer.SetTime(duration);
        newTimer.SetSymbol(symbol);
        newTimer.SetPosition(newPosition);
        newTimer.StartTimer(duration);

        // Add the new timer to the list of active timers
        activeTimers.Add(newTimer);
    }

    void Update()
    {
        // Remove completed timers from the list
        activeTimers.RemoveAll(timer => !timer.IsRunning);
    }
}
