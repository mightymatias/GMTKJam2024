using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] spawnableCrumbs;
    [SerializeField] private KeyCode spawnKey = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(spawnKey)){
            SpawnObject();
        }
    }

    void SpawnObject(){
        if (spawnableCrumbs != null){
            // Right now, crumb spawning is random - This is the code for that
            System.Random random = new System.Random();
            int randomIndex = random.Next(spawnableCrumbs.Length);
            GameObject objectPrefab = spawnableCrumbs[randomIndex];

            // The actual spawning code for crumbs
            GameObject newObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            // Switch new crumbs to the Newly Spawned Crumb layer so we can interact with them differently
            int NSClayer = LayerMask.NameToLayer("Newly Spawned Crumb");
            if (NSClayer != -1){
                newObject.layer = NSClayer;
            }
            // Give the new crumbs gravity so they fall in a satisfying maner
            activateGravity(newObject);
            Debug.Log("Spawned object");
        } else {
            Debug.Log("Object prefab not assigned");
        }
    }

    void activateGravity(GameObject crumb){
        Rigidbody2D rb = crumb.GetComponent<Rigidbody2D>();
        rb.drag = 0;
        rb.gravityScale = 1;
        rb.angularDrag = 0;
    }

    


}
