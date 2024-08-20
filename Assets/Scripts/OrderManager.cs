using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public int totalOrdersTurnedIn;
    public Recipe[] recipeBook;
    public int currentOrderCount = 0;
    public int maxOrderCount = 5;
    public List<GameObject> currentOrders;
    public float orderSpawnInterval = 5f;
    public GameObject orderCardPrefab;
    public float orderShiftAmount = 100f;
    public float orderSlideTime = 1;

    void Start(){
        StartCoroutine(SpawnItems());
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.H)){
            NewOrder();
        }
        if (Input.GetKeyDown(KeyCode.J)){
            DevDestroyOrder();
        }
    }

    IEnumerator SpawnItems(){
        while(true){
            // If the max number of orders is reached, wait
            while (currentOrders.Count >= maxOrderCount){
                yield return null; // Wait for the next frame
            }

            // Create an order
            NewOrder();
            yield return new WaitForSeconds(orderSpawnInterval);
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
        /*----------Get a random recipe to cook----------*/
        Recipe randomRecipe = GetRandomOrder();
        /*-----------Create a new order prefab-----------*/
        GameObject canvasObject = GameObject.Find("Canvas");
        UnityEngine.Vector2 spawnPoint = new UnityEngine.Vector2(-1050,436);
        GameObject newOrderCard = Instantiate(orderCardPrefab, transform.position, UnityEngine.Quaternion.identity);
        newOrderCard.transform.SetParent(canvasObject.transform);
        RectTransform newOrderCardRectTransform = newOrderCard.GetComponent<RectTransform>();
        newOrderCardRectTransform.anchoredPosition = spawnPoint;
        /*------------Set up the order prefab------------*/
        // Set the sprite of the final product
        newOrderCard.transform.Find("Final Product").GetComponent<SpriteRenderer>().sprite = randomRecipe.product.GetComponent<SpriteRenderer>().sprite;
        newOrderCard.GetComponent<Order>().recipe = randomRecipe;
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
        /*-----Move all orders to the left-----*/
        SlideOrderTickets();
    }

    public void DestroyOrder(GameObject product){
        int i = 0;
        int positionToDelete = -1;
        foreach (GameObject order in currentOrders){
            Debug.Log("You gave: " + product + " Order called for: " + order.GetComponent<Order>().recipe.product);
            if (order.GetComponent<Order>().recipe.GameObject().GetComponent<Crumb>().crumbName == product.GameObject().GetComponent<Crumb>().crumbName){
                Debug.Log("Those are the same!");
                positionToDelete = i;
                break;
            }

            i++;
        }

        if (positionToDelete != -1){
            GameObject primedForRemoval = currentOrders[positionToDelete];
            Debug.Log("We're deleting " + primedForRemoval);
            currentOrders.RemoveAt(positionToDelete);
            Destroy(primedForRemoval);
            currentOrderCount--;
            
        }
    }

    public void SlideOrderTickets(){
        UnityEngine.Vector3 rightwardMove = new UnityEngine.Vector3(orderShiftAmount, 0, 0);
        foreach (GameObject orderCard in currentOrders){
            RectTransform orderCardRectTransform = orderCard.GetComponent<RectTransform>();
            UnityEngine.Vector2 targetPosition = orderCardRectTransform.localPosition + rightwardMove;
            StartCoroutine(ActualSlide(targetPosition, orderSlideTime, orderCardRectTransform));
        }
        
    }

    IEnumerator ActualSlide(UnityEngine.Vector2 targetPosition, float duration, RectTransform orderCardRectTransform){
        UnityEngine.Vector2 startPosition = orderCardRectTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration){
            orderCardRectTransform.localPosition = UnityEngine.Vector2.Lerp(startPosition, targetPosition, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        orderCardRectTransform.localPosition = targetPosition;
    }

    public void DevDestroyOrder(){
        Destroy(currentOrders[0]);
        currentOrders.RemoveAt(0);
        currentOrderCount--;
        totalOrdersTurnedIn++;
    }

    

}
