using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void deactivateGravity(GameObject crumb){
        Rigidbody2D rb = crumb.GetComponent<Rigidbody2D>();
        rb.drag = 5;
        rb.gravityScale = 0;
        rb.angularDrag = 5;
    }

    void OnCollisionEnter2D(Collision2D other){
        Debug.Log("Collide!");
        GameObject otherObject = other.gameObject;
        deactivateGravity(otherObject);
        // Set this crumb to the normal crumb layer
        int crumbLayer = LayerMask.NameToLayer("Crumbs");
        if (crumbLayer != -1){
            otherObject.layer = crumbLayer;
        }
    }
}
