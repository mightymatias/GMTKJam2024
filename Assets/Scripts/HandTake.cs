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

            // Sliding back to the original position
            StartCoroutine(SlideToPosition(originalPosition, oneWayMovementDuration));
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

    public void checkForFood(){
        //if food is in the trigger, collect it
        moveFoodtoHand();
    }

    public void moveFoodtoHand(){

    }
        

}
