using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbSpawning : MonoBehaviour
{
    [SerializeField] private GameObject breadCrumbPrefab;
    [SerializeField] private GameObject meatCrumbPrefab;
    [SerializeField] private GameObject cheeseCrumbPrefab;

    // Dictionary to map job names to crumb prefabs
    private Dictionary<string, GameObject> jobToCrumbMap;

    void Start()
    {
        // Initialize the map
        jobToCrumbMap = new Dictionary<string, GameObject>
        {
            { "Bread", breadCrumbPrefab },
            { "Meat", meatCrumbPrefab },
            { "Cheese", cheeseCrumbPrefab }
        };
    }

    // Method to spawn a crumb based on job name
    public void SpawnCrumb(string jobName)
    {
        if (jobToCrumbMap.ContainsKey(jobName))
        {
            GameObject crumbPrefab = jobToCrumbMap[jobName];
            GameObject newObject = Instantiate(crumbPrefab, transform.position, Quaternion.identity);

            // Switch new crumbs to the Newly Spawned Crumb layer so we can interact with them differently
            int NSClayer = LayerMask.NameToLayer("Newly Spawned Crumb");
            if (NSClayer != -1)
            {
                newObject.layer = NSClayer;
            }

            // Give the new crumbs gravity so they fall in a satisfying manner
            ActivateGravity(newObject);
            Debug.Log($"Spawned crumb for job '{jobName}'");
        }
        else
        {
            Debug.LogError($"No crumb prefab found for job '{jobName}'");
        }
    }

    private void ActivateGravity(GameObject crumb)
    {
        Rigidbody2D rb = crumb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.drag = 0;
            rb.gravityScale = 1;
            rb.angularDrag = 0;
        }
    }
}
