using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastMeal: Meal {

    public override string getMealName() { return "Toast"; }

    public override List<string> getRecipe() { 
        return new List<string> { "Bread Crumb" };
    }

}