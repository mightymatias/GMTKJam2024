using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // speed of character, set in editor
    [SerializeField] private float interactionRange = 1000f; // range of player interactions
    private bool isInRange; // range for detecting crumbs
    private bool isInteractReleased = true; //required to avoid double inputs
    private float speedX, speedY; // speed variables for moving
    private Rigidbody2D rb; // rigid body
    private GameObject heldObject, nearbyCrumb; // used for interactions
    private Transform heldObjectOGParent; // remembering the original parent for the held object so we can set it when the object is set down

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){

        if (Input.GetKeyUp(KeyCode.E)){
            isInteractReleased = true;
        }

        //Picking up Items
        if (isInteractReleased && heldObject != null && Input.GetKeyDown(KeyCode.E)){
            Drop();
            isInteractReleased = false;
        }
        if (isInteractReleased && isInRange && Input.GetKeyDown(KeyCode.E) && heldObject == null ){
            PickUp();
            isInteractReleased = false;
        }
    }

    void FixedUpdate() {

        //Player Movement
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        UnityEngine.Vector2 movedir = new UnityEngine.Vector2(speedX, speedY).normalized;
        rb.velocity = movedir * moveSpeed;

        //Debug.Log("inRange?: " + isInRange + " Held Object: " + heldObject);
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Crumb")){
            isInRange = true;
            nearbyCrumb = other.gameObject;
            Debug.Log("Player in Range");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Crumb")){
            isInRange = false;
            nearbyCrumb = null;
            Debug.Log("Player out of Range");
        }
    }

    void PickUp(){
        Debug.Log("Entered pickup");
        //Logic for what happens on pickup
        heldObject = nearbyCrumb;
        nearbyCrumb = null;
        heldObjectOGParent = heldObject.transform.parent;
        heldObject.transform.SetParent(this.transform);
        heldObject.GetComponent<Rigidbody2D>().isKinematic = true;
        heldObject.GetComponent<Rigidbody2D>().simulated = false;
        Debug.Log("Picked up Object");
    }

    void Drop(){
        Debug.Log("Entered drop");
        // Detect nearby interaction stations
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);
        Debug.Log("Nearby colliders: " + colliders.Length + " They are: " + colliders);

        InteractionStation station = null;
        float closestStationDistance = Mathf.Infinity;
        InteractionStation closestStation = null;

        Debug.Log("Searching for station");
        // Loop through all the colliders to find the closest station
        Debug.Log("Nearby colliders: " + colliders.Length + " They are: ");
        foreach(Collider2D collider in colliders){
            Debug.Log("Collider: " + collider);
            station = collider.GetComponent<InteractionStation>(); // Only look deeper into this if the collider belongs to an interaction station
            if (station != null){ // If station exists
                Debug.Log("Found a station");
                float distanceToStation = UnityEngine.Vector2.Distance(transform.position, station.transform.position); // Distance to the station 
                if (distanceToStation < closestStationDistance){ // If this station is the new closest one
                    closestStationDistance = distanceToStation; // Set it to be the closest one
                    closestStation = station;
                    station = null;
                }
            }
        }

        Debug.Log(closestStation + " with distance " + closestStationDistance);

        // If a station was found
        if (closestStation != null){
            // if the crumb can't go there, print a debug log. PROBABLY WANT A UI MESSAGE
            if (!closestStation.CanInteractWithCrumb(heldObject.GetComponent<Crumb>())){
                Debug.Log("Crumb can't interact with this station");
            } else { // Otherwise the crumb can interact with this station, and will be put on the station
                heldObject.transform.position = closestStation.transform.position; // Snap the crumb to the station's position
                UnityEngine.Vector3 newPos = heldObject.transform.position;
                newPos.z -= 1;
                heldObject.transform.position = newPos;
                heldObject.transform.SetParent(closestStation.transform); // Set the crumb's parent to the station
                Debug.Log("Successfully put crumb in station");
                closestStation.StartInteraction(heldObject.GetComponent<Crumb>()); // Start the station interaction
                heldObjectOGParent = null; // Remove the reference to the OG parent as we don't need it anymore
            }  
        }
        // At this point, if we still have an object, it needs to go on the ground
        if( heldObject != null){
            heldObject.transform.SetParent(heldObjectOGParent);
            heldObject.GetComponent<Rigidbody2D>().isKinematic = false;
            heldObject.GetComponent<Rigidbody2D>().simulated = true;
            heldObject = null;
            Debug.Log("Dropped object");
        }
    }
}
