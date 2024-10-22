using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HandTake : MonoBehaviour
{
    // When the hand is spawned, it will automatically perform this

    public Vector2 targetPosition;
    public Vector2 originalPosition;
    public float oneWayMovementDuration = 1.0f;
    void Awake(){
        originalPosition = transform.position;
        StartCoroutine(SlideToPosition(targetPosition, oneWayMovementDuration, () => {
            //Intermediary Code would go here
            //Check to see if the thing picked up is in the order queue. If so, remove it, score a point, etc.


            // Sliding back to the original position
            StartCoroutine(SlideToPosition(originalPosition, oneWayMovementDuration, () => {
                Destroy(gameObject);
            }));
            
        }));
    }

    IEnumerator SlideToPosition(Vector2 targetPosition, float oneWayMovementDuration, Action onComplete = null){
        UnityEngine.Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while(elapsedTime < oneWayMovementDuration){
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime/oneWayMovementDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        onComplete?.Invoke();
    }

    public void OnTriggerEnter2D(Collider2D other){
        GameObject otherObject = other.gameObject;
        otherObject.transform.position = transform.Find("Interact Position").position;
        otherObject.transform.SetParent(transform);

        OrderManager orderManager = FindObjectOfType<OrderManager>();
        orderManager.DestroyOrder(otherObject);
    }
        

}
