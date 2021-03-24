using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Joseph
public class IntroductionE2 : MonoBehaviour
{
    //We need to create a walker
    introMover2 walker;
    movingObject movingObject1;

    // Start is called before the first frame update
    void Start()
    {
        movingObject1 = new movingObject();
        walker = new introMover2(movingObject1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {        //Have the walker choose a direction
        walker.step();
        walker.CheckEdges();
        movingObject1.step();
        movingObject1.CheckEdges();
    }
}

public class movingObject
{
    // The basic properties of a mover class
    private Vector3 locationObj;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject moverObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

    public movingObject()
    {
        findWindowLimits();
        locationObj = Vector2.zero;
        //We need to create a new material for WebGL
        Renderer r = moverObj.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        locationObj = moverObj.transform.position;
        //Each frame choose a new Random number 0,1,2,3, 
        //If the number is equal to one of those values, take a step
        int choice = Random.Range(0, 4);
        if (choice == 0)
        {
            locationObj.x++;

        }
        else if (choice == 1)
        {
            locationObj.x--;
        }
        else if (choice == 3)
        {
            locationObj.y++;
        }
        else
        {
            locationObj.y--;
        }

        moverObj.transform.position += locationObj * Time.deltaTime;
    }

    public void CheckEdges()
    {
        locationObj = moverObj.transform.position;

        if (locationObj.x > maximumPos.x)
        {
            locationObj = Vector2.zero;
        }
        else if (locationObj.x < minimumPos.x)
        {
            locationObj = Vector2.zero;
        }
        if (locationObj.y > maximumPos.y)
        {
            locationObj = Vector2.zero;
        }
        else if (locationObj.y < minimumPos.y)
        {
            locationObj = Vector2.zero;
        }
        moverObj.transform.position = locationObj;
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

public class introMover2
{
    // The basic properties of a mover class
    private Vector3 location;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public movingObject movingOb1;

    public introMover2(movingObject movingOb)
    {
        findWindowLimits();
        location = Vector2.zero;
        movingOb1 = movingOb;
        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        location = mover.transform.position;
        //Each frame choose a new Random number 0,1,2,3, 
        //If the number is equal to one of those values, take a step
        float choice = Random.Range(0f, 1f);
        if (choice >= 0.7f)
        {
            location.x++;
            mover.transform.position = location;

        }
        else if (0.5f < choice && choice < 0.7f)
        {
            location.x--;
            mover.transform.position = location;
        }
        else if (0.3f < choice && choice <= 0.5f)
        {
            location.y++;
            mover.transform.position = location;
        }
        else
        {
            mover.transform.position = Vector3.MoveTowards(location, movingOb1.moverObj.transform.position, Time.deltaTime);
        }
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
