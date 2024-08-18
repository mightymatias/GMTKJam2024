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
        lineRenderer.startWidth = StartingWidth; // Example width
        lineRenderer.endWidth = EndingWidth; // Example width
        lineRenderer.numCapVertices = NumCapVertices; // Rounds the edges
    }

    public void UpdateLine(Vector2 position)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            return;
        }
        if (Vector2.Distance(points.Last(), position) > .05f)
        {
            Vector2 lastPoint = points.Last();
            Vector2 direction = (position - lastPoint).normalized;
            float distance = Vector2.Distance(lastPoint, position);
            int interpolationSteps = Mathf.CeilToInt(distance / Smoothness);

            totalDistance += distance;

            for (int i = 1; i <= interpolationSteps; i++)
            {
                SetPoint(lastPoint + direction * (0.05f * i));
            }
        }
    }

    void SetPoint(Vector2 point)
    {
        points.Add(point);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    public float GetTotalDistance()
    {
        return totalDistance;
    }
}
