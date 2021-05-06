using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem3 : MonoBehaviour
{
    List<oscillator> oscillators = new List<oscillator>();
    Vector3 oscillatorAvgPosition = Vector3.zero;

    private Vector3 location, velocity, acceleration;
    private float topSpeed;

    public float minX;
    public float minY;
    public float minZ;
    public float maxX;
    public float maxY;
    public float maxZ;

    void Start()
    {
        location = this.gameObject.transform.position;
        velocity = Vector3.zero;
        acceleration = new Vector3(Random.Range(-.5f, .5f), Random.Range(-.1f, .1f), Random.Range(-.5f, .5f));
        topSpeed = 1f;
        minX = 0f;
        maxX = 50f;

        minY = 2f;
        maxY = 14f;

        minZ = 0f;
        maxZ = 50f;

        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;
        this.gameObject.transform.localScale = new Vector3(2, 2, 2);
        while (oscillators.Count < 8)
        {
            oscillators.Add(new oscillator(this.gameObject.transform.position));
        }
        foreach (oscillator o in oscillators)
        {
            o.oGameObject.transform.SetParent(this.gameObject.transform);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (oscillator o in oscillators)
        {
            //Each oscillator object oscillating on the x-axis
            float x = Mathf.Sin(o.angle.x) * o.amplitude.x;
            //Each oscillator object oscillating on the y-axis
            float y = Mathf.Sin(o.angle.y) * o.amplitude.y;
            //Add the oscillator's velocity to its angle
            o.angle += o.velocity;
            // Draw the line for each oscillator
            o.lineRender.SetPosition(0, this.gameObject.transform.position);
            o.lineRender.SetPosition(1, o.oGameObject.transform.position);
            o.lineRender.transform.SetParent(this.gameObject.transform);
            //Move the oscillator
            o.oGameObject.transform.transform.Translate(new Vector2(x, y) * Time.deltaTime);
            oscillatorAvgPosition += o.oGameObject.transform.transform.position;
        }
        oscillatorAvgPosition = new Vector2(oscillatorAvgPosition.x / oscillators.Count, oscillatorAvgPosition.y / oscillators.Count);
        //this.gameObject.transform.position = oscillatorAvgPosition;
        PublicMove();
    }

    public void PublicMove()
    {
        location = this.gameObject.transform.position;
        velocity += acceleration; // Time.deltaTime is the time passed since the last frame.

        velocity = Vector3.ClampMagnitude(velocity, topSpeed);

        location += velocity * Time.deltaTime;

        this.gameObject.transform.position = new Vector3(location.x, location.y, location.z);

        CheckEdges();
    }

    void CheckEdges()
    {
        if (location.x > maxX)
        {
            location.x -= maxX - minX;
        }
        else if (location.x < minX)
        {
            location.x += maxX - minX;
        }
        if (location.y > maxY)
        {
            location.y -= maxY - minY;
        }
        else if (location.y < minY)
        {
            location.y += maxY - minY;
        }
        if (location.z > maxZ)
        {
            location.z -= maxZ - minZ;
        }
        else if (location.z < minZ)
        {
            location.z += maxZ - minZ;
        }
        this.gameObject.transform.position = new Vector3(location.x, location.y, location.z);
    }
}

public class centralBody
{
    public GameObject sphereBody;
    public centralBody()
    {
        sphereBody = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer renderer = sphereBody.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;
        sphereBody.transform.localScale = new Vector3(2, 2, 2);
    }
}

public class oscillator
{

    // The basic properties of an oscillator class
    public Vector2 velocity, angle, amplitude;

    // The window limits
    private Vector3 maximumPos = new Vector3(1, 2, 3);

    // Gives the class a GameObject to draw on the screen
    public GameObject oGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    //Create variables for rendering the line between two vectors
    public LineRenderer lineRender;

    public oscillator(Vector3 sphereBodyPosition)
    {
        oGameObject.transform.position = sphereBodyPosition;
        //findWindowLimits();
        angle = Vector2.zero;
        velocity = new Vector2(Random.Range(-.05f, .05f), Random.Range(-0.05f, 0.05f));
        amplitude = new Vector2(Random.Range(-maximumPos.x / 2, maximumPos.x / 2), Random.Range(-maximumPos.y / 3, maximumPos.y / 3));

        //We need to create a new material for WebGL
        Renderer r = oGameObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.color = Color.red;

        // Create a GameObject that will be the line
        GameObject lineDrawing = new GameObject();
        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Diffuse"));
        lineRender.material.color = Color.red;
        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3
        Vector3 center = new Vector3(oGameObject.transform.position.x, oGameObject.transform.position.y, oGameObject.transform.position.z);
        lineRender.SetPosition(0, center);
    }
}
