using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaconMeal: Meal {

    public override string getMealName() { return "Bacon"; }

    public override List<string> getRecipe() { 
        return new List<string> { "Ham Crumb" };
    }

}