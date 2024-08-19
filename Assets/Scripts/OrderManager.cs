using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public Recipe[] recipeBook;
    public int currentOrderCount = 0;
    public int maxOrderCount = 5;
    public Recipe[] currentOrders;
    public float timeBetweenOrderSpawn = 10f;

    void Start(){

    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.H)){
            NewOrder();
        }
    }

    public Recipe GetRecipe(GameObject finalproduct){
        foreach (Recipe recipe in recipeBook){
            if (recipe.product == finalproduct){
                return recipe;
            }
        }
        Debug.LogError("Product is not found in the recipe book");
        return null;
    }

    public Recipe GetRandomOrder(){
        System.Random random = new System.Random();
        int randomIndex = random.Next(recipeBook.Length);
        return recipeBook[randomIndex];
    }

    public void NewOrder(){
        // Get a random recipe to cook
        Recipe randomRecipe = GetRandomOrder();
        // Debug for now, print out the random order and what ingredents it requires
        Debug.Log("Current Order: " + randomRecipe.GetComponent<Crumb>().crumbName);
        Debug.Log("Ingredients are: " + randomRecipe.ingredients);
    }

}
