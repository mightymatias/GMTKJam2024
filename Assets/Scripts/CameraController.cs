using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float cameraSmoothTime = 0.1f;

    private UnityEngine.Vector2 cameraVelocity = UnityEngine.Vector2.zero;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 targetPosition = player.position;
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        // Calculate clamped position based on bounds
        float clampedX = Mathf.Clamp(targetPosition.x, minBounds.x + halfWidth, maxBounds.x + halfWidth);
        float clampedY = Mathf.Clamp(targetPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        // Set camera position to clamped position
        Vector2 cameraTargetPosition = new Vector2(clampedX, clampedY);

        // Smoothly move the camera towards the target position
        Vector2 smoothPosition = Vector2.SmoothDamp(transform.position, cameraTargetPosition, ref cameraVelocity, cameraSmoothTime);

        // Update camera position, keeping z unchanged
        transform.position = new Vector3(smoothPosition.x, smoothPosition.y, transform.position.z);

        
    }
}
