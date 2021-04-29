using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem2 : MonoBehaviour
{

    public Transform transform;
    public Rigidbody body;

    private Vector3 minimumPos, maximumPos;

    // Start is called before the first frame update
    void Start()
    {

        maximumPos = new Vector3(50f, 10f, 50f);
        minimumPos = new Vector3(0f, 2f, 0f);

        Vector3 randomVelocity = new Vector3(Random.Range(0f, 5f), Random.Range(0f, 5f), Random.Range(0f, 5f));

        GameObject.Destroy(this.gameObject.GetComponent<BoxCollider>());
        transform = this.gameObject.transform;
        this.gameObject.AddComponent<Rigidbody>();
        body = this.gameObject.GetComponent<Rigidbody>();
        body.useGravity = false;
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        float randomMass = Random.Range(0.2f, 1f);
        this.gameObject.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        body.mass = 1;
        body.position = this.gameObject.transform.position; // Default location
        body.velocity = randomVelocity; // The extra velocity makes the mover orbit
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 velocity = body.velocity;
        if (transform.position.x > maximumPos.x || transform.position.x < minimumPos.x)
        {
            velocity.x *= -1 * Time.deltaTime;
        }
        if (transform.position.y > maximumPos.y || transform.position.y < minimumPos.y)
        {
            velocity.y *= -1 * Time.deltaTime;
        }
        if (transform.position.z > maximumPos.z || transform.position.z < minimumPos.z)
        {
            velocity.z *= -1 * Time.deltaTime;
        }
        body.velocity = velocity;
    }
}
