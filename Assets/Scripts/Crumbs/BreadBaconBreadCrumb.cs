using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBaconBreadCrumb : ComplexCrumb
{
    // Start is called before the first frame update
    void Start()
    {
        crumbName = "Bread Bacon Bread Crumb";        
    }

    public override void OnPickUp(){
        Debug.Log(crumbName + " picked up!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
