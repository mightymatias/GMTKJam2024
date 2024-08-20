using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionStation : MonoBehaviour
{

    [SerializeField] protected float interactionTime = 5;

    // Starts the interaction process
    public virtual void StartInteraction(Crumb crumb){
        crumb.GetComponent<PolygonCollider2D>().enabled = false;
        crumb.GetComponent<Rigidbody2D>().velocity = new Vector2 (0,0);
        StartCoroutine(InteractionCoroutine(crumb));
    }

    public virtual bool CanInteractWithCrumb(Crumb crumb){
        return true;
    }

    protected virtual IEnumerator InteractionCoroutine(Crumb crumb){
        OnInteractionStart(crumb);
        Debug.Log("Interacting with " + crumb.crumbName);
        yield return new WaitForSeconds(interactionTime);
        OnInteractionComplete(crumb);
    }

    protected abstract void OnInteractionStart(Crumb crumb);

    protected abstract void OnInteractionComplete(Crumb crumb);

    //method for popping up GUI

    //method for interaction start

    //method for interaction complete
}
