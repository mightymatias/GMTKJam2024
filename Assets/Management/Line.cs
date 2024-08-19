using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;

    List<Vector2> points;
    
    public float Smoothness = .2f;

    public float StartingWidth = .1f;
    public float EndingWidth = .1f;
    public int NumCapVertices = 10;

    private float totalDistance = 0f;

    void Start()
    {
        // Set the tag to "Line"
        gameObject.tag = "Line";

        // Set up the LineRenderer properties
        lineRenderer.startWidth = StartingWidth;
        lineRenderer.endWidth = EndingWidth;
        lineRenderer.numCapVertices = NumCapVertices;
        lineRenderer.sortingLayerName = "Foreground";
    }

    public float UpdateLine(Vector2 position)
    {
        float distanceAdded = 0f;

        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            return 0f;
        }

        if (Vector2.Distance(points.Last(), position) > .05f)
        {
            Vector2 lastPoint = points.Last();
            Vector2 direction = (position - lastPoint).normalized;
            float distance = Vector2.Distance(lastPoint, position);
            int interpolationSteps = Mathf.CeilToInt(distance / Smoothness);

            totalDistance += distance;
            distanceAdded = distance;

            for (int i = 1; i <= interpolationSteps; i++)
            {
                SetPoint(lastPoint + direction * (0.05f * i));
            }
        }

        return distanceAdded;
    }

    void SetPoint(Vector2 point)
    {
        points.Add(point);

        // Force Z to be in front of other objects
        Vector3 pointWithZ = new Vector3(point.x, point.y, -1f); // Adjust -1f if necessary
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, pointWithZ);
    }

    public float GetTotalDistance()
    {
        return totalDistance;
    }
}
