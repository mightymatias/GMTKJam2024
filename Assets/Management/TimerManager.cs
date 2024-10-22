using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public GameObject[] timerPrefabs; // Prefab of the timer with the Text UI component

    // Method to create a new timer and return the Timer component
    public Timer CreateTimer()
    {
        GameObject newTimerObject = Instantiate(timerPrefabs[Random.Range(0, timerPrefabs.Length)], transform);
        return newTimerObject.GetComponent<Timer>();
    }

    // Method to create and start a timer with a specified position and duration
    public Timer CreateAndStartTimer(Vector3 position, float duration)
    {
        Timer newTimer = CreateTimer();
        newTimer.SetPosition(position);
        newTimer.StartTimer(duration);
        return newTimer;
    }
}
