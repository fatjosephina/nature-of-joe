using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem2 : MonoBehaviour
{
    List<Ecosystem2Mover> movers = new List<Ecosystem2Mover>(); // Now we have multiple Movers!
    EcosystemAttractor a;

    private Predator predator;

    // Start is called before the first frame update
    void Start()
    {
        predator = new Predator();
        int numberOfMovers = 10;
        for (int i = 0; i < numberOfMovers; i++)
        {
            Vector2 randomLocation = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
            Vector2 randomVelocity = new Vector2(Random.Range(0f, 5f), Random.Range(0f, 5f));
            Ecosystem2Mover m = new Ecosystem2Mover(Random.Range(0.2f, 1f), randomVelocity, randomLocation); //Each Mover is initialized randomly.
            movers.Add(m);
        }
        a = new EcosystemAttractor();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Ecosystem2Mover m in movers)
        {
            Rigidbody body = m.body;
            Vector2 force = a.Attract(body) + predator.Repel(body); // Apply the attraction from the Attractor on each Mover object
            m.ApplyForce(force);
            m.Update();
        }
    }

    void Update()
    {
        predator.Update();
        predator.CheckEdges();
    }
}


public class EcosystemAttractor
{
    public float mass;
    private Vector2 location;
    private float G;
    public Rigidbody body;
    private GameObject attractor;
    private float radius;

    public EcosystemAttractor()
    {
        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.Destroy(attractor.GetComponent<SphereCollider>());
        Renderer renderer = attractor.GetComponent<Renderer>();
        body = attractor.AddComponent<Rigidbody>();
        body.position = Vector2.zero;

        // Generate a radius
        radius = 2;

        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        attractor.transform.position = body.position;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        attractor.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
        body.useGravity = false;
        body.isKinematic = true;

        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;

        G = 9.8f;
    }

    public Vector2 Attract(Rigidbody m)
    {
        Vector2 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (G * body.mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }
}

public class Ecosystem2Mover
{
    // The basic properties of a mover class
    public Transform transform;
    public Rigidbody body;

    private Vector2 minimumPos, maximumPos;

    private GameObject mover;

    public Ecosystem2Mover(float randomMass, Vector2 initialVelocity, Vector2 initialPosition)
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.Destroy(mover.GetComponent<SphereCollider>());
        transform = mover.transform;
        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.useGravity = false;
        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        mover.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        body.mass = 1;
        body.position = initialPosition; // Default location
        body.velocity = initialVelocity; // The extra velocity makes the mover orbit
        findWindowLimits();





    }

    public void ApplyForce(Vector2 force)
    {
        body.AddForce(force, ForceMode.Force);
    }

    public void Update()
    {
        CheckEdges();
    }

    public void CheckEdges()
    {
        Vector2 velocity = body.velocity;
        if (transform.position.x > maximumPos.x || transform.position.x < minimumPos.x)
        {
            velocity.x *= -1 * Time.deltaTime;
        }
        if (transform.position.y > maximumPos.y || transform.position.y < minimumPos.y)
        {
            velocity.y *= -1 * Time.deltaTime;
        }
        body.velocity = velocity;
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

public class Predator
{
    // The basic properties of a mover class
    private Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 minimumPos, maximumPos;


    // Gives the class a GameObject to draw on the screen
    private GameObject predator = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    private float G;
    public Rigidbody body;

    public Predator()
    {
        findWindowLimits();
        location = Vector2.zero; // Vector2.zero is a (0, 0) vector
        velocity = Vector2.zero;
        acceleration = new Vector2(-0.1F, -1F);
        topSpeed = 10F;
        Renderer renderer = predator.GetComponent<Renderer>();

        body = predator.AddComponent<Rigidbody>();


        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        predator.transform.position = body.position;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        predator.transform.localScale = 2 * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = 4f * Mathf.PI;
        body.useGravity = false;
        body.isKinematic = true;

        //We need to create a new material for WebGL
        Renderer r = predator.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.blue;
        G = 9.8f;
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
        predator.transform.position = new Vector2(location.x, location.y);
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

    public Vector2 Repel(Rigidbody m)
    {
        Vector2 force = m.position - body.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 50f);

        force.Normalize();
        float strength = (G * body.mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }
}
