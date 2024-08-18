using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInStation : InteractionStation
{
    [SerializeField] private GameObject[] spawnableHands;
    [SerializeField] public Vector2 spawnPosition;
    protected override void OnInteractionComplete(Crumb crumb){
        crumb.OnCooked();
        Debug.Log(crumb.crumbName + " is cooked in the oven!");
    }

    protected override void OnInteractionStart(Crumb crumb){
        Debug.Log(crumb.crumbName + " has started interaction");
        SpawnObject();
        //spawn a hand
    }

    public override bool CanInteractWithCrumb(Crumb crumb){
        return crumb.isFinalProduct; // Returns true if crumb has a cooked version
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
