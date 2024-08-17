using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Crumb : MonoBehaviour
{

    public GameObject cookedVersion; //Reference to the cooked version of the crumb
    public GameObject cutVersion; //Reference to the cut version of the crumb
    public GameObject skeweredVersion; //Reference to the skewered version of the crumb
    public string crumbName; //Name of the crumb

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Abstract method to define what happens when the crumb is picked up
    public abstract void OnPickUp();

    // Method to get called when the crumb is cooked
    public void OnCooked(){
        if (cookedVersion != null){
            Instantiate(cookedVersion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        } else {
            Debug.LogError(crumbName + " does not have a cooked version assigned!");
        }
    }

    public void OnCut(){
        if (cutVersion != null){
            Instantiate(cutVersion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        } else {
            Debug.LogError(crumbName + " does not have a cut version assigned!");
        }
    }

    public void OnSkewer(){
        if (skeweredVersion != null){
            Instantiate(skeweredVersion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        } else {
            Debug.LogError(crumbName + " does not have a skewered version assigned!");
        }
    }
        
}
