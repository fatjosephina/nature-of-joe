using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3E2 : MonoBehaviour
{
    Mover3_2 mover;
    public GameObject cannon;
    Vector2 velocity = Vector2.zero;
    Vector3 force = new Vector3(0f, 5f, 10f);
    bool shouldAddForce = false;
    Vector3 gravity = new Vector3(0f, -9.8f, 0f);
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mover = new Mover3_2(0.5f, velocity, cannon.transform.position);

            shouldAddForce = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shouldAddForce)
        {
            mover.body.AddForce(force, ForceMode.Impulse);
            mover.body.MoveRotation(mover.constrainAngularMotion(force));
            shouldAddForce = false;
        }
        if (mover != null)
        {
            mover.body.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}

public class Mover3_2
{
    public Rigidbody body;
    private GameObject gameObject;
    public Transform transform;

    private Vector3 angle;
    private Quaternion angleRotation;

    private Vector2 minimumPos, maximumPos;

    public Mover3_2(float randomMass, Vector2 initialVelocity, Vector2 initialPosition)
    {
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.Destroy(gameObject.GetComponent<BoxCollider>());
        transform = gameObject.transform;
        gameObject.AddComponent<Rigidbody>();
        body = gameObject.GetComponent<Rigidbody>();
        body.useGravity = false;
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;
        gameObject.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        body.mass = randomMass;
        body.position = initialPosition; // Default location
        body.velocity = initialVelocity; // The extra velocity makes the mover orbit
        findWindowLimits();

    }
    //Constrain the forces with (arbitrary) angular motion

    public Quaternion constrainAngularMotion(Vector3 angularForce)
    {
        //Calculate angular acceleration according to the acceleration's X horizontal direction and magnitude
        Vector3 aAcceleration = new Vector3(angularForce.x, angularForce.y, angularForce.z);
        Quaternion bodyRotation = body.rotation;
        bodyRotation.eulerAngles += new Vector3(aAcceleration.x, aAcceleration.y, aAcceleration.z);
        bodyRotation.x = Mathf.Clamp(bodyRotation.x, aAcceleration.y, bodyRotation.z);
        angle += bodyRotation.eulerAngles * Time.deltaTime;
        angleRotation = Quaternion.Euler(angle.x, angle.y, angle.z);
        return angleRotation;
    }

    //Checks to ensure the body stays within the boundaries
    public void CheckEdges()
    {
        Vector2 velocity = body.velocity;
        if (body.position.x > maximumPos.x || body.position.x < minimumPos.x)
        {
            velocity.x *= -1 * Time.deltaTime; ;
        }
        if (body.position.y > maximumPos.y || body.position.y < minimumPos.y)
        {
            velocity.y *= -1 * Time.deltaTime; ;
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
