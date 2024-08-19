using System.Collections;
using System.Collections.Generic;
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
        } else {
            crumb = lastCombination;
        }
        
        
    }

    protected bool isInteractionComplete(Crumb crumb){
        return crumb.isFinalProduct;
    }

    public override bool CanInteractWithCrumb(Crumb crumb){
        foreach (int tier in crumb.combinationTiers){
            Debug.Log("**!!**!!Crumb Tier: " + tier + "|Station tier: " + currentTier);
            if (tier == currentTier){
                Debug.Log("Can Interact");
                currentTier++;
                return true;
            }
        }
        return false;
    }

    public Crumb TryToCombine(Crumb crumb, Crumb previousCrumb){
        foreach(ComplexCrumb interaction in interactionPossibilities){
            if (interaction.ingredient1 == previousCrumb && interaction.ingredient2 == crumb){
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
