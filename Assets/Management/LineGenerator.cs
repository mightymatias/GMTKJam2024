using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public UIManager uiManager;

    public GameObject breadDestination;
    public GameObject meatDestination;
    public GameObject cheeseDestination;

    public AntManagement antManagement;
    public Sprite breadSymbol;
    public Sprite meatSymbol;
    public Sprite cheeseSymbol;

    private Dictionary<GameObject, string> destinationToJobMap =
        new Dictionary<GameObject, string>();
    private HashSet<string> activeConnections = new HashSet<string>(); // Track active connections

    public GameObject linePrefab;
    public GameObject canvasObject;
    public GameObject leftObject; // Object that controls canvas movement
    public GameObject nestObject; // The nest from where the lines originate

    public float snapRadius = 0.5f; // Radius for snapping
    public Color normalLineColor = Color.black; // Normal color of the line
    public Color outOfInkColor = Color.red; // Color of the line when out of ink

    Line activeLine;
    public List<Line> allLines = new List<Line>();

    public float minimumCanvasY = 0f;

    private Vector2 previousLeftPosition;
    private Vector2 previousNestPosition;
    public float maxInk = 10f;
    private float currentInk;

    void Start()
    {
        previousLeftPosition = leftObject.transform.position;
        previousNestPosition = nestObject.transform.position;

        maxInk = antManagement.totalNPCWorkers; // Set maxInk based on totalNPCWorkers
        currentInk = maxInk; // Initialize ink

        // Initialize destination-to-job mapping
        destinationToJobMap[breadDestination] = "Bread";
        destinationToJobMap[meatDestination] = "Meat";
        destinationToJobMap[cheeseDestination] = "Cheese";

        // Update UI
        uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk);
    }

    void Update()
    {
        // Update the position of the left object and the nest
        Vector2 currentLeftPosition = leftObject.transform.position;
        Vector2 leftDelta = currentLeftPosition - previousLeftPosition;

        if (leftDelta != Vector2.zero)
        {
            MoveLinesWithCanvas(leftDelta);
            previousLeftPosition = currentLeftPosition;
        }

        // Check if the canvas is within the minimum allowed Y position
        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        float currentCanvasY = canvasRect.anchoredPosition.y;

        if (currentCanvasY < minimumCanvasY)
        {
            return;
        }

        // Handle starting a new line
        if (Input.GetMouseButtonDown(0) && currentInk > 0f)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = ClampToCanvas(mousePos);

            // Ensure the line starts at the nest
            if (Vector2.Distance(mousePos, nestObject.transform.position) <= snapRadius)
            {
                // Get the nearest snap point to check if a connection already exists
                GameObject snappedObject = GetClosestSnapPoint(mousePos);

                if (snappedObject != null && destinationToJobMap.ContainsKey(snappedObject))
                {
                    string jobName = destinationToJobMap[snappedObject];
                    if (activeConnections.Contains(jobName))
                    {
                        Debug.Log(
                            $"A connection to {jobName} already exists. Cannot draw another line."
                        );
                        return; // Exit early to prevent line creation
                    }
                }

                // Proceed with creating a new line
                GameObject newLine = Instantiate(linePrefab, canvasObject.transform);
                activeLine = newLine.GetComponent<Line>();
                allLines.Add(activeLine);

                // Set the initial color of the line
                SetLineColor(normalLineColor);

                activeLine.UpdateLine(nestObject.transform.position); // Start line at nest
            }
        }

        // Handle finishing a line
        if (Input.GetMouseButtonUp(0))
        {
            if (activeLine != null)
            {
                float totalDistance = activeLine.GetTotalDistance();
                if (totalDistance > currentInk)
                {
                    Debug.Log(
                        $"Line exceeds ink limit. Deleting line with distance: {totalDistance}"
                    );
                    allLines.Remove(activeLine);
                    Destroy(activeLine.gameObject);
                }
                else
                {
                    currentInk -= totalDistance;
                    Debug.Log($"Total Distance: {totalDistance}. Remaining Ink: {currentInk}");

                    // Check if the line connects to a valid destination
                    GameObject closestSnapPoint = GetClosestSnapPoint(activeLine.GetEndPosition());
                    if (
                        closestSnapPoint != null
                        && destinationToJobMap.ContainsKey(closestSnapPoint)
                    )
                    {
                        string jobName = destinationToJobMap[closestSnapPoint];

                        // Start the job if it's a valid, unique connection
                        if (!activeConnections.Contains(jobName))
                        {
                            StartJobForConnection(jobName);
                        }
                        else
                        {
                            Debug.Log(
                                $"A connection to {jobName} is already active. Deleting line."
                            );
                            allLines.Remove(activeLine);
                            Destroy(activeLine.gameObject);
                        }
                    }
                }

                // Update the UI to reflect the final state after drawing
                uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk);
            }
            activeLine = null;
        }

        // Handle updating an ongoing line
        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = ClampToCanvas(mousePos);

            GameObject snappedObject = GetClosestSnapPoint(mousePos);
            if (snappedObject != null)
            {
                mousePos = snappedObject.transform.position;
            }

            float addedDistance = activeLine.UpdateLine(mousePos);

            // Predict the total distance that will be used
            float potentialTotalDistance = activeLine.GetTotalDistance() + addedDistance;

            // Update the line color if the ink will run out
            if (potentialTotalDistance > currentInk)
            {
                SetLineColor(outOfInkColor);
            }
            else
            {
                SetLineColor(normalLineColor);
            }

            // Calculate the predicted ink after drawing this segment
            float inkAfterDrawing = currentInk - potentialTotalDistance;

            // Update the UI in real-time to reflect the number of active workers
            uiManager.UpdateActiveWorkersBasedOnInk(maxInk, inkAfterDrawing);
        }

        // Handle deleting the oldest line (if right-clicked)
        if (Input.GetMouseButtonDown(1))
        {
            DeleteOldestLine();
        }
    }

    GameObject GetClosestSnapPoint(Vector2 position)
    {
        GameObject[] snapPoints = GameObject.FindGameObjectsWithTag("SnapPoint");
        GameObject closestPoint = null;
        float closestDistance = snapRadius;

        foreach (GameObject snapPoint in snapPoints)
        {
            float distance = Vector2.Distance(position, snapPoint.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = snapPoint;
            }
        }

        return closestPoint;
    }

    void MoveLinesWithCanvas(Vector2 leftDelta)
    {
        foreach (Line line in allLines)
        {
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                Vector3 currentPosition = lineRenderer.GetPosition(i);
                Vector3 newPosition = currentPosition + (Vector3)leftDelta;
                lineRenderer.SetPosition(i, newPosition);
            }
        }
    }

    Vector2 ClampToCanvas(Vector2 position)
    {
        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);

        Vector2 canvasMin = new Vector2(canvasCorners[0].x, canvasCorners[0].y);
        Vector2 canvasMax = new Vector2(canvasCorners[2].x, canvasCorners[2].y);

        float clampedX = Mathf.Clamp(position.x, canvasMin.x, canvasMax.x);
        float clampedY = Mathf.Clamp(position.y, canvasMin.y, canvasMax.y);
        return new Vector2(clampedX, clampedY);
    }

    void DeleteOldestLine()
    {
        if (allLines.Count > 0)
        {
            DeleteLine(allLines[allLines.Count - 1]);
        }
    }

    void DeleteLine(Line line)
    {
        float lengthOfLine = line.GetTotalDistance();
        Debug.Log($"Deleting Line with Length: {lengthOfLine}");

        // Check if the line is connected to a job destination
        GameObject closestSnapPoint = GetClosestSnapPoint(line.GetEndPosition());
        if (closestSnapPoint != null && destinationToJobMap.ContainsKey(closestSnapPoint))
        {
            string jobName = destinationToJobMap[closestSnapPoint];
            if (activeConnections.Contains(jobName))
            {
                activeConnections.Remove(jobName);
                // Instead of stopping the job immediately, flag it for stopping after completion
                antManagement.FlagJobForStopAfterCompletion(jobName);
                Debug.Log($"Flagged connection to {jobName} for stopping after job completion.");
            }
        }

        allLines.Remove(line);
        Destroy(line.gameObject);

        // Refund ink
        currentInk += lengthOfLine;
        currentInk = Mathf.Min(currentInk, maxInk); // Ensure ink does not exceed max
        Debug.Log($"Ink refunded. Current Ink: {currentInk}");
    }

    void SetLineColor(Color color)
    {
        if (activeLine != null)
        {
            LineRenderer lineRenderer = activeLine.GetComponent<LineRenderer>();
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }

    public void UpdateInk(int availableWorkers)
    {
        maxInk = antManagement.totalNPCWorkers;
        currentInk = Mathf.Min(currentInk, availableWorkers);

        Debug.Log($"Ink updated. Max Ink: {maxInk}, Current Ink: {currentInk}");

        // Update UI
        uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk);
    }

    public bool IsDrawingLine()
    {
        return activeLine != null;
    }

    void StartJobForConnection(string jobName)
    {
        Sprite jobSymbol = null;

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

        // Calculate the number of workers needed based on the length of the active line
        float lineLength = activeLine.GetTotalDistance();
        int workersForJob = Mathf.CeilToInt(lineLength);
        float jobDuration = 60f; // Define job duration

        // Assign workers and start the job in AntManagement
        antManagement.AssignWorkersToJob(jobName, workersForJob, jobDuration, jobSymbol);

        // Mark this connection as active
        activeConnections.Add(jobName);

        // Debug.Log($"Job {jobName} started with {workersForJob} workers for {jobDuration} seconds.");
    }
}
