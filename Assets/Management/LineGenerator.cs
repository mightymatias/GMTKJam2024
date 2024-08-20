using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public JobManager jobManager;
    public UIManager uiManager;
    public GameObject breadDestination;
    public GameObject meatDestination;
    public GameObject cheeseDestination;

    public Sprite breadSymbol;
    public Sprite meatSymbol;
    public Sprite cheeseSymbol;

    private Dictionary<GameObject, string> destinationToJobMap =
        new Dictionary<GameObject, string>();
    private HashSet<string> activeConnections = new HashSet<string>();

    public GameObject linePrefab;
    public GameObject canvasObject;
    public GameObject nestObject;
    public float snapRadius = 0.5f;

    Line activeLine;
    public List<Line> allLines = new List<Line>();

    public float maxInk = 10f;
    private float currentInk;

    void Start()
    {
        destinationToJobMap[breadDestination] = "Bread";
        destinationToJobMap[meatDestination] = "Meat";
        destinationToJobMap[cheeseDestination] = "Cheese";

        maxInk = jobManager.totalNPCWorkers;
        currentInk = maxInk;

        uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk);
    }

    void Update()
    {
        // Ensure that drawing is only attempted if the mouse is over the correct area
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(mousePos, nestObject.transform.position) <= snapRadius)
            {
                // Start drawing a line
                StartDrawingLine(mousePos);
            }
        }

        if (Input.GetMouseButton(0) && activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdateDrawingLine(mousePos);
        }

        if (Input.GetMouseButtonUp(0) && activeLine != null)
        {
            FinishDrawingLine();
        }

        if (Input.GetMouseButtonDown(1))
        {
            DeleteOldestLine();
        }
    }

    private void StartDrawingLine(Vector2 startPos)
    {
        // Create a new line only if the user is clicking near the nestObject
        if (activeLine == null)
        {
            GameObject newLine = Instantiate(linePrefab, canvasObject.transform);
            activeLine = newLine.GetComponent<Line>();
            allLines.Add(activeLine);
            activeLine.UpdateLine(startPos);
        }
    }

    private void UpdateDrawingLine(Vector2 currentPos)
    {
        if (activeLine != null)
        {
            GameObject snappedObject = GetClosestSnapPoint(currentPos);

            if (snappedObject != null)
            {
                currentPos = snappedObject.transform.position;
            }

            activeLine.UpdateLine(currentPos);

            // Calculate the total distance of the line so far
            float totalDistance = activeLine.GetTotalDistance();

            // Calculate the predicted workers based on the total distance (e.g., 1 worker per unit distance)
            int predictedWorkers = Mathf.CeilToInt(totalDistance);

            // Update the UI to reflect the predicted number of workers for the current line length
            uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk - totalDistance);

            if (totalDistance > currentInk)
            {
                // Change line color to indicate insufficient workers/ink
                activeLine.SetLineColor(activeLine.insufficientInkColor);
            }
            else
            {
                // Reset to the default color if sufficient workers/ink are available
                activeLine.SetLineColor(activeLine.defaultColor);
            }
        }
    }

    private void FinishDrawingLine()
    {
        if (activeLine != null)
        {
            float totalDistance = activeLine.GetTotalDistance();

            if (totalDistance <= currentInk)
            {
                currentInk -= totalDistance;

                // Connect the line to a valid job destination
                GameObject closestSnapPoint = GetClosestSnapPoint(activeLine.GetEndPosition());
                if (closestSnapPoint != null && destinationToJobMap.ContainsKey(closestSnapPoint))
                {
                    string jobName = destinationToJobMap[closestSnapPoint];
                    if (!activeConnections.Contains(jobName))
                    {
                        Sprite jobSymbol = GetJobSymbol(jobName);
                        if (
                            jobManager.TryAssignWorkersToJob(
                                jobName,
                                Mathf.CeilToInt(totalDistance),
                                20f,
                                jobSymbol,
                                activeLine.GetInstanceID().ToString()
                            )
                        )
                        {
                            activeConnections.Add(jobName);
                        }
                    }
                }
            }
            else
            {
                // If the line exceeds the ink limit, delete it
                allLines.Remove(activeLine);
                Destroy(activeLine.gameObject);
            }

            // Clear the active line once finished
            activeLine = null;

            // Update the UI to reflect the final worker count after line creation
            uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk);
        }
    }

    public bool IsDrawingLine()
    {
        return activeLine != null;
    }

    private Sprite GetJobSymbol(string jobName)
    {
        switch (jobName)
        {
            case "Bread":
                return breadSymbol;
            case "Meat":
                return meatSymbol;
            case "Cheese":
                return cheeseSymbol;
            default:
                return null;
        }
    }

    private GameObject GetClosestSnapPoint(Vector2 position)
    {
        GameObject[] snapPoints = new GameObject[]
        {
            breadDestination,
            meatDestination,
            cheeseDestination,
        };
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

    private Vector2 ClampToCanvas(Vector2 position)
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

    private void DeleteOldestLine()
    {
        if (allLines.Count > 0)
        {
            Line lineToDelete = allLines[allLines.Count - 1];
            GameObject closestSnapPoint = GetClosestSnapPoint(lineToDelete.GetEndPosition());

            if (closestSnapPoint != null && destinationToJobMap.ContainsKey(closestSnapPoint))
            {
                string jobName = destinationToJobMap[closestSnapPoint];
                if (activeConnections.Contains(jobName))
                {
                    activeConnections.Remove(jobName);
                    jobManager.FlagJobForStopAfterCompletion(jobName);

                    // Ensure the job is also removed from the jobs dictionary if necessary
                    jobManager.RemoveConnection(jobName);
                }
            }

            allLines.Remove(lineToDelete);
            Destroy(lineToDelete.gameObject);
            currentInk += lineToDelete.GetTotalDistance();
            uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk);
        }
    }
}
