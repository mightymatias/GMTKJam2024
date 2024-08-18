using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheesePorkBurgerMeal: Meal {

    public override string getMealName() { return "Cheese Pork Burger"; }

    public override List<string> getRecipe() {
        return new List<string> { "Bread Crumb", "Ham Crumb", "Cheese Crumb" };
    }

}