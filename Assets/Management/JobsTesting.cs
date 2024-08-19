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

    [Header("Timer Prefab Settings")]
    public GameObject timerPrefab; // Reference to the Timer prefab
    public Transform parentCanvasTransform; // Reference to the parent Canvas or UI panel

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
                return;
        }

        // Instantiate the Timer prefab and parent it to the correct Canvas
        GameObject newTimer = Instantiate(timerPrefab, parentCanvasTransform);
        newTimer.transform.localPosition = timerPosition; // Set the position relative to the parent
        newTimer.transform.localScale = Vector3.one; // Reset scale to ensure consistency

        // Set the job symbol in the image component
        Image jobImage = newTimer.GetComponentInChildren<Image>();
        jobImage.sprite = jobSymbol;
        jobImage.SetNativeSize(); // Adjust size to match the sprite resolution

        // Call the method to assign workers to the job in AntManagement with the appropriate symbol
        antManager.AssignWorkersToJob(jobName, workersForJob, jobDuration, jobSymbol);
    }
}
