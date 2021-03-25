﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1E4 : MonoBehaviour
{
    // Declare a mover object
    private Mover1_8 mover;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Mover object
        mover = new Mover1_8();
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        mover.Update();
        mover.CheckEdges();
    }
}

public class Mover1_8
{
    // The basic properties of a mover class
    private Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 minimumPos, maximumPos;


    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_8()
    {
        findWindowLimits();
        location = Vector2.zero; // Vector2.zero is a (0, 0) vector
        velocity = Vector2.zero;
        acceleration = new Vector2(-0.1F, -1F);
        topSpeed = 10F;

        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Update()
    {
        // Speeds up the mover
        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

        // Limit Velocity to the top speed
        velocity = Vector2.ClampMagnitude(velocity, topSpeed);

        // Moves the mover
        location += velocity * Time.deltaTime;


        // Updates the GameObject of this movement
        mover.transform.position = new Vector2(location.x, location.y);
    }

    public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
    {
        // magnitude = sqrt of x^2 * y^2. sqrMagnitude = magnitude ^2. => sqrMagnitude = x^2 * y^2
        if ((Mathf.Pow(vector.x, 2f) + Mathf.Pow(vector.y, 2f)) > Mathf.Pow(maxLength, 2f))
        {
            return vector.normalized * maxLength;
        }
        return vector;
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x -= maximumPos.x - minimumPos.x;
        }
        else if (location.x < minimumPos.x)
        {
            location.x += maximumPos.x - minimumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y -= maximumPos.y - minimumPos.y;
        }
        else if (location.y < minimumPos.y)
        {
            location.y += maximumPos.y - minimumPos.y;
        }
    }

    private void findWindowLimits()
    {
        // The code to find the information on the camera as seen in Figure 1.2

        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
