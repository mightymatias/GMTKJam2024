using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : InteractionStation
{

    public Animator animator; //this connects the animator so it knows when to play what animation

    protected override void OnInteractionStart(Crumb crumb)
    {
        animator.SetFloat("AnimFire", 1);
    }

    protected override void OnInteractionComplete(Crumb crumb){
        crumb.OnCooked();
        Debug.Log(crumb.crumbName + " is cooked in the oven!");
        animator.SetFloat("AnimFire", 0);
    }

    public override bool CanInteractWithCrumb(Crumb crumb){
        return crumb.cookedVersion != null; // Returns true if crumb has a cooked version
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
