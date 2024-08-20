using UnityEngine;

public class LineFollower : MonoBehaviour
{
    public RectTransform canvasRectTransform;
    private Vector2 lastCanvasPosition;
    private LineGenerator lineGenerator;

    void Start()
    {
        if (canvasRectTransform == null)
        {
            canvasRectTransform = GetComponent<RectTransform>();
        }

        lastCanvasPosition = canvasRectTransform.anchoredPosition;
        lineGenerator = GetComponentInChildren<LineGenerator>();
    }

    void Update()
    {
        Vector2 canvasMovement = canvasRectTransform.anchoredPosition - lastCanvasPosition;

        if (canvasMovement != Vector2.zero && lineGenerator != null)
        {
            // Adjust all lines based on the canvas movement
            foreach (Line line in lineGenerator.allLines)
            {
                RectTransform lineRect = line.GetComponent<RectTransform>();
                lineRect.anchoredPosition += canvasMovement;
            }
        }

        lastCanvasPosition = canvasRectTransform.anchoredPosition;
    }
}
