using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpawning : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnableHands;
    [SerializeField] public Vector2 spawnPosition;
    [SerializeField] private KeyCode spawnKey = KeyCode.F;
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
        if (spawnableHands != null){
            // Get a random hand
            System.Random random = new System.Random();
            int randomIndex = random.Next(spawnableHands.Length);
            GameObject spawnObject = spawnableHands[randomIndex];
            // Hand Spawn position

            // Spawn the hand
            GameObject newObject = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
        }
    }
}
