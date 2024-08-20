// using UnityEngine;
// using UnityEngine.UI;

// public class JobsTesting : MonoBehaviour
// {
//     public Button startJobButton;
//     public AntManagement antManager;

//     [Header("Job Settings")]
//     public string jobName = "Bread";
//     public int workersForJob = 5;
//     public float jobDuration = 60f;
//     public Sprite breadSymbol;
//     public Sprite meatSymbol;
//     public Sprite cheeseSymbol;

//     void Start()
//     {
//         startJobButton.onClick.AddListener(StartJob);
//     }

//     void StartJob()
//     {
//         Sprite jobSymbol = null;

//         // Determine which symbol to use based on job name
//         switch (jobName)
//         {
//             case "Bread":
//                 jobSymbol = breadSymbol;
//                 break;
//             case "Meat":
//                 jobSymbol = meatSymbol;
//                 break;
//             case "Cheese":
//                 jobSymbol = cheeseSymbol;
//                 break;
//             default:
//                 Debug.LogError("Job name not recognized: " + jobName);
//                 return;
//         }

//         // Directly assign workers to the job, let the DynamicTimerController handle timer creation
//         antManager.AssignWorkersToJob(jobName, workersForJob, jobDuration, jobSymbol);
//     }
// }
