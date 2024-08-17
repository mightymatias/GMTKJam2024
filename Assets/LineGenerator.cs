using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject canvasObject; // Reference to the canvas GameObject
    
    Line activeLine;
    
    public List<Line> allLines = new List<Line>();

    // Define the boundaries of your canvas (set these values based on your specific canvas size)
    public Vector2 canvasMin = new Vector2(-5f, -5f); // Example lower-left corner
    public Vector2 canvasMax = new Vector2(5f, 5f);   // Example upper-right corner

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLine = Instantiate(linePrefab);
            activeLine = newLine.GetComponent<Line>();
            allLines.Add(activeLine); // Add the newly created line to the list
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (activeLine != null)
            {
                // Get the total distance and log it
                float totalDistance = activeLine.GetTotalDistance();
                Debug.Log($"Total Distance: {totalDistance}");
            }
            activeLine = null;
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"Current Position: {mousePos}");
            // Clamp the mouse position within the canvas boundaries
            mousePos = ClampToCanvas(mousePos);

            activeLine.UpdateLine(mousePos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            DeleteOldestLine();
        }
    }

    Vector2 ClampToCanvas(Vector2 position)
    {
        // Get the RectTransform of the canvas GameObject
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
            // Find the oldest line (first in the list)
            Line oldestLine = allLines[0];

            // Log the length of the line being deleted
            float lengthOfOldestLine = oldestLine.GetTotalDistance();
            Debug.Log($"Deleting Oldest Line with Length: {lengthOfOldestLine}");

            // Remove it from the list and destroy its GameObject
            allLines.RemoveAt(0);
            Destroy(oldestLine.gameObject);
        }
    }
}
