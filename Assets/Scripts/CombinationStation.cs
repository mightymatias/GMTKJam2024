using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombinationStation : InteractionStation
{
    int currentTier = 0;
    public Crumb lastCombination;
    public ComplexCrumb[] interactionPossibilities;
    protected override void OnInteractionStart(Crumb crumb)
    {

    }

    protected override void OnInteractionComplete(Crumb crumb){
        crumb.OnPlace();
        Debug.Log(crumb.crumbName + " is placed on the station!");
        // tier 0 is two normal crumbs. Every tier after is a crumb and a complex crumb
        if (currentTier > 0){
            Crumb combinedCrumb = TryToCombine(crumb, lastCombination);
            if (combinedCrumb != null){
                Destroy(crumb.gameObject);
                Destroy(lastCombination.gameObject);
                Instantiate(combinedCrumb.GameObject(), transform.Find("Interact Position").position, Quaternion.identity);
                lastCombination = null;
                currentTier = 0;
            }
        } else {
            lastCombination = crumb;
            currentTier++;
        }
        
        
        
    }

    protected bool isInteractionComplete(Crumb crumb){
        return crumb.isFinalProduct;
    }

    public override bool CanInteractWithCrumb(Crumb crumb){
        Debug.Log("CURRENT TIER: " + currentTier);
        // if this is the first ingredient in (of 2), check all ingredient 1's to see if it's on the list
        if (currentTier == 0){
            Debug.Log(crumb.crumbName);
            foreach (ComplexCrumb interaction in interactionPossibilities){
                Debug.Log(interaction.ingredient1.GetComponent<Crumb>().crumbName);
                if (interaction.ingredient1.GetComponent<Crumb>().crumbName == crumb.crumbName){
                    Debug.Log("returning true ing1");
                    return true;
                }
            }
            return false;
        } else { // else this is ingredient 2, and we do the same thing, but with ingredient 2's
            foreach (ComplexCrumb interaction in interactionPossibilities){
                Debug.Log("..searching ingredient 2");
                if (interaction.ingredient2.GetComponent<Crumb>().crumbName == crumb.crumbName){
                    Debug.Log("returning true ing2");
                    return true;
                }
            }
            return false;
        }
    }

    public Crumb TryToCombine(Crumb crumb, Crumb previousCrumb){
        Debug.Log(previousCrumb.crumbName + " | " + crumb.crumbName);
        foreach(ComplexCrumb interaction in interactionPossibilities){
            Debug.Log(interaction + " is made from " + interaction.ingredient1.GetComponent<Crumb>().crumbName + " and " + interaction.ingredient2.GetComponent<Crumb>().crumbName);
            if (interaction.ingredient1.GetComponent<Crumb>().crumbName == previousCrumb.crumbName && interaction.ingredient2.GetComponent<Crumb>().crumbName == crumb.crumbName){
                return interaction;
            }
        }
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
