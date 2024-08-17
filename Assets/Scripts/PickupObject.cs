using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class PickupObject : MonoBehaviour
{

    private bool isInRange = false;
    private bool isHoldingObject = false;
    private GameObject player;
    private Transform originalParent;


    void Start(){
        originalParent = transform.parent;
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
            isInRange = true;
            player = other.gameObject;
            Debug.Log("Player in Range");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")){
            isInRange = false;
            player = null;
            Debug.Log("Player out of Range");
        }
    }

    void PickUp(){
        Debug.Log("Picked up Object");
        isHoldingObject = true;
        //Logic for what happens on pickup
        transform.SetParent(player.transform);
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void Drop(){
        Debug.Log("Dropped Object");
        isHoldingObject = false;
        //Logic for what happens on pickup
        transform.SetParent(originalParent);
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E)){
            if (isHoldingObject){
                Drop();
            } else{
                PickUp();
            }
        }
    }
}
