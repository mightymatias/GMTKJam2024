using UnityEngine;

[ExecuteInEditMode]
public class SnapPointGizmo : MonoBehaviour
{
    public float snapRadius = .75f;
    public Color gizmoColor = Color.green;

    void OnDrawGizmos()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Convert the RectTransform's position to world position
            Vector3 worldPosition = rectTransform.TransformPoint(rectTransform.rect.center);

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(worldPosition, snapRadius);
        }
        else
        {
            // Fallback for non-UI elements
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, snapRadius);
        }
    }
}
