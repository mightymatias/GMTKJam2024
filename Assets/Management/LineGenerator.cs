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
        if (activeLine == null)
        {
            GameObject closestSnapPoint = GetClosestSnapPoint(startPos);

            if (closestSnapPoint != null && destinationToJobMap.ContainsKey(closestSnapPoint))
            {
                string jobName = destinationToJobMap[closestSnapPoint];

                // Prevent starting a new line if there's already an active connection to this job
                if (activeConnections.Contains(jobName))
                {
                    Debug.Log("A connection to this destination already exists.");
                    return;
                }
            }

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

            float totalDistance = activeLine.GetTotalDistance();
            int predictedWorkers = Mathf.CeilToInt(totalDistance);

            uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk - totalDistance);

            if (totalDistance > currentInk)
            {
                activeLine.SetLineColor(activeLine.insufficientInkColor);
            }
            else
            {
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
                    else
                    {
                        // If the job already has an active connection, destroy the line
                        DestroyLine(activeLine);
                    }
                }
            }
            else
            {
                DestroyLine(activeLine);
            }

            activeLine = null;
            uiManager.UpdateActiveWorkersBasedOnInk(maxInk, currentInk);
        }
    }

    private void DestroyLine(Line line)
    {
        if (line != null)
        {
            allLines.Remove(line);
            Destroy(line.gameObject);
            currentInk += line.GetTotalDistance();
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
