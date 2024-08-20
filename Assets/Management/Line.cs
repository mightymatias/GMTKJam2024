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
        lineRenderer.sortingLayerName = "UI";
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

        // Ensure only points sufficiently far apart are added
        if (Vector2.Distance(points.Last(), position) > .05f)
        {
            Vector2 lastPoint = points.Last();
            float distance = Vector2.Distance(lastPoint, position);

            // Update the total distance directly
            totalDistance += distance;
            distanceAdded = distance;

            // Interpolate and add points if needed
            Vector2 direction = (position - lastPoint).normalized;
            int interpolationSteps = Mathf.CeilToInt(distance / Smoothness);

            for (int i = 1; i <= interpolationSteps; i++)
            {
                SetPoint(lastPoint + direction * (distance / interpolationSteps) * i);
            }
        }

        return distanceAdded;
    }

    void SetPoint(Vector2 point)
    {
        points.Add(point);

        // Force Z to be in front of other objects
        Vector3 pointWithZ = new Vector3(point.x, point.y, -1f);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, pointWithZ);
    }

    public float GetTotalDistance()
    {
        return totalDistance;
    }

    public Vector2 GetEndPosition()
    {
        if (points != null && points.Count > 0)
        {
            return points.Last();
        }
        return Vector2.zero; // Return a default value if no points are set
    }
}
