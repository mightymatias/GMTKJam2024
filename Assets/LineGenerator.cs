using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject canvasObject;
    public GameObject leftObject; // Object that controls canvas movement
    public GameObject nestObject; // The nest from where the lines originate
    public float snapRadius = 0.75f; // Radius for snapping

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
        currentInk = maxInk; // Initialize ink
    }

    void Update()
    {
        Vector2 currentLeftPosition = leftObject.transform.position;
        Vector2 leftDelta = currentLeftPosition - previousLeftPosition;

        if (leftDelta != Vector2.zero)
        {
            MoveLinesWithCanvas(leftDelta);
            previousLeftPosition = currentLeftPosition;
        }

        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        float currentCanvasY = canvasRect.anchoredPosition.y;

        if (currentCanvasY < minimumCanvasY)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && currentInk > 0f)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = ClampToCanvas(mousePos);

            // Ensure the line starts at the nest
            if (Vector2.Distance(mousePos, nestObject.transform.position) <= snapRadius)
            {
                GameObject newLine = Instantiate(linePrefab, canvasObject.transform);
                activeLine = newLine.GetComponent<Line>();
                allLines.Add(activeLine);
                activeLine.UpdateLine(nestObject.transform.position); // Start line at nest
            }
        }

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
                }
            }
            activeLine = null;
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = ClampToCanvas(mousePos);

            GameObject snappedObject = GetClosestSnapPoint(mousePos);
            if (snappedObject != null)
            {
                mousePos = snappedObject.transform.position;
            }

            activeLine.UpdateLine(mousePos);
        }

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
            DeleteLine(allLines[0]);
        }
    }

    void DeleteLine(Line line)
    {
        float lengthOfLine = line.GetTotalDistance();
        Debug.Log($"Deleting Line with Length: {lengthOfLine}");

        allLines.Remove(line);
        Destroy(line.gameObject);

        // Refund ink
        currentInk += lengthOfLine;
        currentInk = Mathf.Min(currentInk, maxInk); // Ensure ink does not exceed max
        Debug.Log($"Ink refunded. Current Ink: {currentInk}");
    }
}
