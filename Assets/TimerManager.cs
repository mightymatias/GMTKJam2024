using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public GameObject timerPrefab; // Prefab of the timer with the Text UI component

    // Method to create a new timer and return the Timer component
    public Timer CreateTimer()
    {
        GameObject newTimerObject = Instantiate(timerPrefab, transform);
        return newTimerObject.GetComponent<Timer>();
    }
}
