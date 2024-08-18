using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkewerStation : InteractionStation
{

    protected override void OnInteractionStart(Crumb crumb)
    {
        Debug.Log("Started SkewerStation");
    }


    protected override void OnInteractionComplete(Crumb crumb){
        crumb.OnSkewer();
        Debug.Log(crumb.crumbName + " is skewered on the skewer station!");
    }
    public override bool CanInteractWithCrumb(Crumb crumb){
        return crumb.skeweredVersion != null; // Returns true if crumb has a skewered version
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
