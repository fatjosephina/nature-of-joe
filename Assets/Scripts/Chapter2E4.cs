﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Joseph, Eric, Dat
public class Chapter2E4 : MonoBehaviour
{
    // Geometry defined in the inspector.
    public float floorY;
    public float leftWallX;
    public float rightWallX;
    public Transform moverSpawnTransform;
    public Vector3 frictionWallPosition;

    private List<Mover2_4> Movers = new List<Mover2_4>();
    // Define constant forces in our environment
    private Vector3 wind = new Vector3(0.002f, 0f, 0f);
    private float frictionStrength = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject frictionWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frictionWall.transform.position = frictionWallPosition;
        frictionWall.transform.localScale = new Vector3(5, 0.2f, 1);
        frictionWall.tag = "Friction Wall";
        // Create copys of our mover and add them to our list
        while (Movers.Count < 30)
        {
            Movers.Add(new Mover2_4(
                moverSpawnTransform.position,
                leftWallX,
                rightWallX,
                floorY
            ));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply the forces to each of the Movers
        foreach (Mover2_4 mover in Movers)
        {
            // ForceMode.Impulse takes mass into account
            mover.body.AddForce(wind, ForceMode.Impulse);

            // Apply a friction force that directly opposes the current motion
            Vector3 friction = mover.body.velocity;
            friction.Normalize();
            friction *= -frictionStrength;
            mover.body.AddForce(friction, ForceMode.Force);

            mover.CheckBoundaries();
        }
    }
}

public class Mover2_4
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;

    public Mover2_4(Vector3 position, float xMin, float xMax, float yMin)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;

        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();
        // Remove functionality that comes with the primitive that we don't want
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        // Generate random properties for this mover
        radius = Random.Range(0.1f, 0.4f);

        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        gameObject.transform.position = position + Vector3.up * radius;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        gameObject.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
    }

    // Checks to ensure the body stays within the boundaries
    public void CheckBoundaries()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - radius < yMin)
        {
            // Using the absolute value here is and important safe
            // guard for the scenario that it takes multiple ticks
            // of FixedUpdate for the mover to return to its boundaries.
            // The intuitive solution of flipping the velocity may result
            // in the mover not returning to the boundaries and flipping
            // direction on every tick.
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
        }
        if (body.position.x - radius < xMin)
        {
            restrainedVelocity.x = Mathf.Abs(restrainedVelocity.x);
        }
        else if (body.position.x + radius > xMax)
        {
            restrainedVelocity.x = -Mathf.Abs(restrainedVelocity.x);
        }
        body.velocity = restrainedVelocity;
    }
}
