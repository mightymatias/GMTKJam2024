using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheesePorkBurgerCrumb : Crumb
{
    // Start is called before the first frame update
    void Start()
    {
        crumbName = "Cheese Porkbuger Crumb"; 
        //isFInalProduct = true;      
    }

    public override void OnPickUp(){
        Debug.Log(crumbName + " picked up!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
