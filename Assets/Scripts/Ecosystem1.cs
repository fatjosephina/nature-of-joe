using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem1 : MonoBehaviour
{
    // Declare a mover object
    private EcoMover mover;
    // The basic properties of a mover class
    private Vector3 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector3 minimumPos, maximumPos;
    float slither = 2f;
    float timer = 2f;

    public float minX;
    public float minY;
    public float minZ;
    public float maxX;
    public float maxY;
    public float maxZ;
    

    // Start is called before the first frame update
    void Start()
    {
        minX = 0f;
        maxX = 50f;

        minY = 5f;
        maxY = 14f;

        minZ = 0f;
        maxZ= 50f;
        // Create a Mover object
        mover = new EcoMover();
        location = this.gameObject.transform.position; // Vector2.zero is a (0, 0) vector
        velocity = Vector3.zero;
        acceleration = new Vector3(-0.1F, 0f, -0.1F);
        topSpeed = 10F;
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        timer -= Time.deltaTime;
        //Debug.Log(timer);
        if (0f < timer && timer <= 1f)
            slither *= -1f;
        else if (timer <= 0f)
            timer = 2f; slither *= -1f;
        acceleration.x = 0f;
        acceleration.x += slither;
        velocity.x = 0f;
        // Speeds up the mover
        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

        velocity.x += slither;
        // Limit Velocity to the top speed
        velocity = Vector3.ClampMagnitude(velocity, topSpeed);

        // Moves the mover
        location += velocity * Time.deltaTime;

        // Updates the GameObject of this movement
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
    }
}

public class EcoMover
{
    // The basic properties of a mover class
    private Vector3 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 minimumPos, maximumPos;


    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public EcoMover()
    {
        //findWindowLimits();
        location = Vector3.zero; // Vector2.zero is a (0, 0) vector
        velocity = Vector3.zero;
        acceleration = new Vector3(-0.1F, 0f, -0.1F);
        topSpeed = 10F;

        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    float slither = 2f;
    float timer = 2f;

    public void Update()
    {
        timer -= Time.deltaTime;
        Debug.Log(timer);
        if (0f < timer && timer <= 1f)
            slither *= -1f;
        else if (timer <= 0f)
            timer = 2f; slither *= -1f;
        acceleration.x = 0f;
        acceleration.x += slither;
        velocity.x = 0f;
        // Speeds up the mover
        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

        velocity.x += slither;
        // Limit Velocity to the top speed
        velocity = Vector3.ClampMagnitude(velocity, topSpeed);

        // Moves the mover
        location += velocity * Time.deltaTime;

        // Updates the GameObject of this movement
        mover.transform.position = new Vector3(location.x, location.y, location.z);
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
