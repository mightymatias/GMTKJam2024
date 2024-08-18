// using UnityEngine;

// public class TimerController : MonoBehaviour
// {
//     public TimerManager timerManager;
//     public Sprite statusSymbol1; // Assign these in the Unity Editor
//     public Sprite statusSymbol2;
//     public Sprite statusSymbol3;

//     private List<Timer> activeTimers = new List<Timer>();

//     void Update()
//     {
//         // Example logic for dynamically managing timers
//         if (Input.GetKeyDown(KeyCode.Alpha1))
//         {
//             CreateTimer(30f, statusSymbol1, new Vector3(100, 100, 0));
//         }
//         if (Input.GetKeyDown(KeyCode.Alpha2))
//         {
//             CreateTimer(45f, statusSymbol2, new Vector3(200, 100, 0));
//         }
//         if (Input.GetKeyDown(KeyCode.Alpha3))
//         {
//             CreateTimer(60f, statusSymbol3, new Vector3(300, 100, 0));
//         }
//     }

//     void CreateTimer(float duration, Sprite symbol, Vector3 position)
//     {
//         if (activeTimers.Count >= 3)
//         {
//             Debug.LogWarning("Maximum of 3 timers are allowed on the screen at the same time.");
//             return;
//         }

//         Timer newTimer = timerManager.CreateTimer();
//         newTimer.SetTime(duration);
//         newTimer.SetSymbol(symbol);
//         newTimer.SetPosition(position);
//         newTimer.StartTimer(duration);

//         activeTimers.Add(newTimer);
//     }
// }
