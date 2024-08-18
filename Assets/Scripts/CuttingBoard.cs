using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : InteractionStation
{

    public Animator animator; //this connects the animator so it knows when to play what animation

    protected override void OnInteractionStart(Crumb crumb)
    {
        animator.SetFloat("AnimCutting", 1); //this starts the cutting animation
    }


    protected override void OnInteractionComplete(Crumb crumb){
        crumb.OnCut();
        Debug.Log(crumb.crumbName + " is cut on the cutting board!");
        animator.SetFloat("AnimCutting", 0);
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
