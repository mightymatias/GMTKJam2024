using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class Recipe : MonoBehaviour
{

    public GameObject product;
    public GameObject[] ingredients;
    public float allotedTimeToCook;

    public Recipe(GameObject product, GameObject[] ingredients, float cookTime){
        this.product = product;
        this.ingredients = ingredients;
        this.allotedTimeToCook = cookTime;
    }
}
