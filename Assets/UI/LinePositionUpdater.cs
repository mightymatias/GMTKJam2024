using UnityEngine;

public class LinePositionUpdater : MonoBehaviour
{
    public RectTransform movingCanvas; // The RectTransform of the moving canvas
    public LineGenerator lineGenerator; // Reference to your LineGenerator

    private Vector3 lastCanvasPosition;

    void Start()
    {
        if (movingCanvas == null)
        {
            Debug.LogError("Moving canvas reference is missing.");
            return;
        }

        lastCanvasPosition = movingCanvas.position;

        if (lineGenerator == null)
        {
            Debug.LogError("LineGenerator reference is missing.");
            return;
        }
    }

    void Update()
    {
        Vector3 canvasMovement = movingCanvas.position - lastCanvasPosition;

        if (canvasMovement != Vector3.zero)
        {
            // Move each line's position in world space to reflect the canvas movement
            foreach (Line line in lineGenerator.allLines)
            {
                for (int i = 0; i < line.lineRenderer.positionCount; i++)
                {
                    Vector3 linePosition = line.lineRenderer.GetPosition(i);
                    line.lineRenderer.SetPosition(i, linePosition + canvasMovement);
                }
            }
        }

        lastCanvasPosition = movingCanvas.position;
    }
}
