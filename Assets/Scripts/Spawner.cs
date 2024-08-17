using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject objectPrefab;
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
        if (objectPrefab != null){
            Instantiate(objectPrefab, transform.position, Quaternion.identity);
            Debug.Log("Spawned object");
        } else {
            Debug.Log("Object prefab not assigned");
        }
    }
}
