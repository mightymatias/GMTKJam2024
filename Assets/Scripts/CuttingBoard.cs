using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : InteractionStation
{

    protected override void OnInteractionComplete(Crumb crumb){
        crumb.OnCut();
        Debug.Log(crumb.crumbName + " is cut on the cutting board!");
    }

    public override bool CanInteractWithCrumb(Crumb crumb){
        return crumb.cutVersion != null; // Returns true if crumb has a cut version
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
