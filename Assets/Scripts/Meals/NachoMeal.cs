using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NachoMeal: Meal {

    public override string getMealName() { return "Nacho"; }

    public override List<string> getRecipe() { 
        return new List<string> { "Bread Crumb", "Cheese Crumb" };
    }

}