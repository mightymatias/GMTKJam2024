using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Meal {

    public abstract string getMealName(); //name of the meal

    public abstract List<string> getRecipe(); //crumbs required to complete the meal

}