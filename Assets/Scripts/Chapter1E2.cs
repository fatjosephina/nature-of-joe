using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Joseph
public class Chapter1E2 : MonoBehaviour
{
    //We need to create a walker
    chapter1Mover3 walker;
    chapter1FoodObject foodObject1;

    // Start is called before the first frame update
    void Start()
    {
        foodObject1 = new chapter1FoodObject();
        walker = new chapter1Mover3(foodObject1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {        //Have the walker choose a direction
        walker.step();
        walker.CheckEdges();
    }
}

public class chapter1FoodObject
{
    public Vector3 location;

    // Gives the class a GameObject to draw on the screen
    public GameObject mealObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

    public chapter1FoodObject()
    {
        location = Vector3.zero;
    }
}

public class chapter1Mover3
{
    // The basic properties of a mover class
    private Vector2 location;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public chapter1FoodObject foodOb1;

    public chapter1Mover3(chapter1FoodObject foodOb)
    {
        findWindowLimits();
        location = Vector2.zero;
        foodOb1 = foodOb;
        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        location = mover.transform.position;
        Vector2 velocity;
        //Each frame choose a new Random number 0,1,2,3, 
        //If the number is equal to one of those values, take a step
        float choice = Random.Range(0f, 1f);
        if (choice >= 0.5f)
        {
            velocity = Vector2.MoveTowards(location, foodOb1.mealObj.transform.position, Time.deltaTime);
        }
        else if (0.375f <= choice && choice < 0.5f)
        {
            velocity = new Vector2(location.x++, location.y);
            mover.transform.position = location;

        }
        else if (0.25f <= choice && choice < 0.375f)
        {
            velocity = new Vector2(location.x--, location.y);
            mover.transform.position = location;
        }
        else if (0.125f <= choice && choice < 0.25f)
        {
            velocity = new Vector2(location.x, location.y++);
            mover.transform.position = location;
        }
        else
        {
            velocity = new Vector2(location.x, location.y--);
            mover.transform.position = location;
        }

        location += velocity;
    }

    public void CheckEdges()
    {
        location = mover.transform.position;

        if (location.x > maximumPos.x)
        {
            location = Vector2.zero;
        }
        else if (location.x < minimumPos.x)
        {
            location = Vector2.zero;
        }
        if (location.y > maximumPos.y)
        {
            location = Vector2.zero;
        }
        else if (location.y < minimumPos.y)
        {
            location = Vector2.zero;
        }
        mover.transform.position = location;
    }

    private void findWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
