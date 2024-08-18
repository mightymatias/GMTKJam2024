using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeweredCheeseMeal: Meal {

    public override string getMealName() { return "Cheese on Skewer"; }

    public override List<string> getRecipe() { 
        return new List<string> { "Cheese Crumb" };
    }

}