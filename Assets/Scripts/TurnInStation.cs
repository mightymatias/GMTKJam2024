using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInStation : InteractionStation
{
    protected override void OnInteractionComplete(Crumb crumb){
        crumb.OnCooked();
        Debug.Log(crumb.crumbName + " is cooked in the oven!");
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
}
