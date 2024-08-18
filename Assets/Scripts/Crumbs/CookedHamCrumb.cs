using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedHamCrumb : Crumb
{
    // Start is called before the first frame update
    void Start()
    {
        crumbName = "Cooked Ham Crumb";        
    }

    public override void OnPickUp(){
        Debug.Log(crumbName + " picked up!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
