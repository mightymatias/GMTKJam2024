using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public Recipe[] recipeBook;
    public int currentOrderCount = 0;
    public int maxOrderCount = 5;
    public List<GameObject> currentOrders;
    public float timeBetweenOrderSpawn = 10f;
    public GameObject orderCardPrefab;
    public float orderShiftAmount = 1.5f;

    void Start(){

    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.H)){
            NewOrder();
        }
        if (Input.GetKeyDown(KeyCode.J)){
            DevDestroyOrder();
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
        /*-----Move all the other orders to the left-----*/
        UnityEngine.Vector3 leftwardMove = new UnityEngine.Vector3(orderShiftAmount, 0);
        foreach (GameObject orderCard in currentOrders){
            orderCard.transform.position += leftwardMove;
        }

        /*----------Get a random recipe to cook----------*/

        Recipe randomRecipe = GetRandomOrder();

        /*-----------Create a new order prefab-----------*/
        GameObject canvasObject = GameObject.Find("Canvas");

        // TODO: FIGURE OUT WHERE THESE ARE GOING ON THE UI AND HOW TO MAKE THEM PUSH EACH OTHER
        UnityEngine.Vector2 spawnPoint = new UnityEngine.Vector2(-850,436);
        GameObject newOrderCard = Instantiate(orderCardPrefab, transform.position, Quaternion.identity);
        newOrderCard.transform.SetParent(canvasObject.transform);
        RectTransform newOrderCardRectTransform = newOrderCard.GetComponent<RectTransform>();
        newOrderCardRectTransform.anchoredPosition = spawnPoint;


        /*------------Set up the order prefab------------*/

        // Set the sprite of the final product
        newOrderCard.transform.Find("Final Product").GetComponent<SpriteRenderer>().sprite = randomRecipe.product.GetComponent<SpriteRenderer>().sprite;
        // Getting references to the ingredient panels
        GameObject ingredient1, ingredient2, ingredient3, ingredient4;
        ingredient1 = newOrderCard.transform.Find("Ingredient1").gameObject;
        ingredient2 = newOrderCard.transform.Find("Ingredient2").gameObject;
        ingredient3 = newOrderCard.transform.Find("Ingredient3").gameObject;
        ingredient4 = newOrderCard.transform.Find("Ingredient4").gameObject;
        // Making an array of cardIngredients to load with the recipe ingredients
        GameObject[] cardIngredients = {ingredient1, ingredient2, ingredient3, ingredient4};
        int i = 0;
        // Loading the cardIngredient sprites with the recipe ingredient sprites
        foreach (GameObject ingredient in randomRecipe.ingredients){
            cardIngredients[i].GetComponent<SpriteRenderer>().enabled = true;
            cardIngredients[i].GetComponent<SpriteRenderer>().sprite = ingredient.GetComponent<SpriteRenderer>().sprite;
            i++;
        }

        /*---Add the order prefab to the array of orders---*/

        currentOrders.Add(newOrderCard);
        currentOrderCount++;
    }

    public void DestroyOrder(GameObject product){
        GameObject remove = currentOrders.Find(orderCard => {
            var orderScript = orderCard.GetComponent<Recipe>();
            return orderScript != null && orderScript.product == product;
        });

        if (remove != null){
            currentOrders.Remove(remove);
            Destroy(remove);
            currentOrderCount--;
        }
    }

    public void DevDestroyOrder(){
        Destroy(currentOrders[0]);
        currentOrders.RemoveAt(0);
        currentOrderCount--;
    }

}
