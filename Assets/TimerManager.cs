using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public GameObject timerPrefab; // Prefab of the timer with the Text UI component

    // Method to create and start a new timer
    public void CreateAndStartTimer(Vector3 position, float duration)
    {
        GameObject newTimerObject = Instantiate(timerPrefab, position, Quaternion.identity, transform);

        Timer timerComponent = newTimerObject.GetComponent<Timer>();
        timerComponent.StartTimer(duration);
    }
}
