using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorkBurgerMeal: Meal {

    public override string getMealName() { return "Pork Burger"; }

    public override List<string> getRecipe() { 
        return new List<string> { "Bread Crumb", "Ham Crumb" };
    }

}