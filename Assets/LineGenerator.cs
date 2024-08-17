using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject canvasObject; // Reference to the Canvas GameObject
    public GameObject leftObject; // Reference to the Left GameObject

    Line activeLine;
    public List<Line> allLines = new List<Line>();

    // Define the minimum Y coordinate required for line drawing to start
    public float minimumCanvasY = 0f;

    // To track the previous position of the Left object
    private Vector2 previousLeftPosition;

    // Start is called before the first frame update
    void Start()
    {
        previousLeftPosition = leftObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current position of the Left object
        Vector2 currentLeftPosition = leftObject.transform.position;

        // Calculate the movement delta of the Left object
        Vector2 leftDelta = currentLeftPosition - previousLeftPosition;

        // If the Left object has moved, update the positions of all lines
        if (leftDelta != Vector2.zero)
        {
            MoveLinesWithCanvas(leftDelta);
            previousLeftPosition = currentLeftPosition;
        }

        // Get the current Y position of the Canvas (attached to the Left object)
        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        float currentCanvasY = canvasRect.anchoredPosition.y;

        if (currentCanvasY < minimumCanvasY)
        {
            // Debug.Log("Canvas is below the required Y position, no drawing allowed.");
            return; // Exit Update early to prevent any line drawing
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLine = Instantiate(linePrefab, canvasObject.transform); // Parent to Canvas
            activeLine = newLine.GetComponent<Line>();
            allLines.Add(activeLine); // Add the newly created line to the list
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (activeLine != null)
            {
                float totalDistance = activeLine.GetTotalDistance();
                Debug.Log($"Total Distance: {totalDistance}");
            }
            activeLine = null;
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = ClampToCanvas(mousePos);
            activeLine.UpdateLine(mousePos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            DeleteOldestLine();
        }
    }
    void MoveLinesWithCanvas(Vector2 leftDelta)
    {
        foreach (Line line in allLines)
        {
            // Get the LineRenderer component from the line
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

            // Loop through all points in the LineRenderer and update their positions
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                // Get the current position of the point
                Vector3 currentPosition = lineRenderer.GetPosition(i);

                // Update the position by applying the movement delta
                Vector3 newPosition = currentPosition + (Vector3)leftDelta;

                // Set the new position back to the LineRenderer
                lineRenderer.SetPosition(i, newPosition);
            }
        }
    }




    Vector2 ClampToCanvas(Vector2 position)
    {
        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();

        // Convert the corners of the RectTransform to world space
        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);

        // Define the canvas bounds
        Vector2 canvasMin = new Vector2(canvasCorners[0].x, canvasCorners[0].y); // Bottom-left corner
        Vector2 canvasMax = new Vector2(canvasCorners[2].x, canvasCorners[2].y); // Top-right corner

        // Clamp the x and y coordinates within the canvas bounds
        float clampedX = Mathf.Clamp(position.x, canvasMin.x, canvasMax.x);
        float clampedY = Mathf.Clamp(position.y, canvasMin.y, canvasMax.y);
        return new Vector2(clampedX, clampedY);
    }

    void DeleteOldestLine()
    {
        if (allLines.Count > 0)
        {
            Line oldestLine = allLines[0];
            float lengthOfOldestLine = oldestLine.GetTotalDistance();
            Debug.Log($"Deleting Oldest Line with Length: {lengthOfOldestLine}");

            allLines.RemoveAt(0);
            Destroy(oldestLine.gameObject);
        }
    }
}
