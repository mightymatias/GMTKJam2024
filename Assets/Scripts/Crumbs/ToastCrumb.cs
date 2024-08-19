using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastCrumb : Crumb
{
    // Start is called before the first frame update
    void Start()
    {
        crumbName = "Toast Crumb";      
        isFinalProduct = true;
    }

    public override void OnPickUp(){
        Debug.Log(crumbName + " picked up!");
    }


    void Update()
    {

    }

}
