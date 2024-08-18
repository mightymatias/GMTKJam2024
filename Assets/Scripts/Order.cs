using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order: MonoBehaviour {

    private List<Meal> possibleMeals = new List<Meal> {
        new BaconMeal(),
        new CheesePorkBurgerMeal(),
        new NachoMeal(),
        new PorkBurgerMeal(),
        new SkeweredCheeseMeal(),
        new ToastMeal()
    };

    public float moveSpeed = 2;

    private Rigidbody2D rbdy;

    private Vector2 endPosition = new Vector2(4, -4);

    private Meal meal;

    void Start() {
        rbdy = GetComponent<Rigidbody2D>();
        transform.position = new Vector2(4, 4);

        System.Random rnd = new System.Random(); 
        int rndIdx = rnd.Next(possibleMeals.Count);
        Meal randomMeal = possibleMeals[rndIdx];

        Debug.Log("The meal for this order is "+randomMeal.getMealName()+"!!!");
    }

    void Update() {
        if (rbdy.position != endPosition) {
            Vector2 newPosition = Vector2.MoveTowards(rbdy.position, endPosition, moveSpeed * Time.deltaTime);
            rbdy.MovePosition(newPosition);
        }
    }

}