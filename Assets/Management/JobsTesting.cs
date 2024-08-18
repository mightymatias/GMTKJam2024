using UnityEngine;
using UnityEngine.UI;

public class JobsTesting : MonoBehaviour
{
    public Button startJobButton;
    public AntManagement antManager;

    [Header("Job Settings")]
    public string jobName = "Bread";
    public int workersForJob = 5;
    public float jobDuration = 60f;
    public Sprite breadSymbol;
    public Sprite meatSymbol;
    public Sprite cheeseSymbol;
    public Vector3 timerPosition = new Vector3(100, 100, 0);

    void Start()
    {
        startJobButton.onClick.AddListener(StartJob);
    }

    void StartJob()
    {
        Sprite jobSymbol = null;

        // Determine which symbol to use based on job name
        switch (jobName)
        {
            case "Bread":
                jobSymbol = breadSymbol;
                break;
            case "Meat":
                jobSymbol = meatSymbol;
                break;
            case "Cheese":
                jobSymbol = cheeseSymbol;
                break;
            default:
                Debug.LogError("Job name not recognized: " + jobName);
                break;
        }

        if (jobSymbol == null)
        {
            Debug.LogError("Job symbol is null! Check the assignment in the inspector.");
        }
        else
        {
            Debug.Log("Selected job symbol: " + jobSymbol.name);
        }

        // Call the method to assign workers to the job in AntManagement with the appropriate symbol
        antManager.AssignWorkersToJob(jobName, workersForJob, jobDuration, jobSymbol);
    }
}
