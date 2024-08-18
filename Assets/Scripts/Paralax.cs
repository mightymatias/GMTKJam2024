using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{

    public Transform cameraTransform;
    public float parallaxFactor = 0.5f;

    private Vector3 previousCameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        previousCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate camera movement
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        // Move forground object based on movement and parallax factor
        transform.position -= new Vector3(deltaMovement.x * parallaxFactor, 0, 0);

        // Update previous camera position
        previousCameraPosition = cameraTransform.position;
        
    }
}
