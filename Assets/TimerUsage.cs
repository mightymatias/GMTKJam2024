using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUsage : MonoBehaviour
{
    public TimerManager timerManager;

    void Start()
    {
        // Example of creating multiple timers
        timerManager.CreateAndStartTimer(new Vector3(0, 0, 0), 30f); // Create a timer at position (0, 0, 0) for 30 seconds
        timerManager.CreateAndStartTimer(new Vector3(0, -50, 0), 60f); // Create another timer at position (0, -50, 0) for 60 seconds
    }
}
