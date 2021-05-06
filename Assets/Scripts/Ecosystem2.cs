using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem2 : MonoBehaviour
{

    public Rigidbody body;

    private Vector3 minimumPos, maximumPos;

    // Start is called before the first frame update
    void Start()
    {

        maximumPos = new Vector3(50f, 14f, 50f);
        minimumPos = new Vector3(0f, 8f, 0f);

        Vector3 randomVelocity = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));

        GameObject.Destroy(this.gameObject.GetComponent<BoxCollider>());
        this.gameObject.AddComponent<Rigidbody>();
        body = this.gameObject.GetComponent<Rigidbody>();
        body.useGravity = false;
        body.constraints = RigidbodyConstraints.FreezeRotation;
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        float randomMass = Random.Range(0.2f, 1f);
        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x * randomMass, this.gameObject.transform.localScale.y * randomMass, this.gameObject.transform.localScale.z * randomMass);

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
            velocity.x *= -1;
        }
        if (transform.position.y > maximumPos.y || transform.position.y < minimumPos.y)
        {
            velocity.y *= -1;
        }
        if (transform.position.z > maximumPos.z || transform.position.z < minimumPos.z)
        {
            velocity.z *= -1;
        }
        body.velocity = velocity;
        lookForward();
    }

    private void lookForward()
    {
        Vector3 velocity = body.velocity;
        Vector3 futureLocation = transform.position + velocity;
        transform.LookAt(futureLocation); // We can use the built in 'LookAt' function to automatically face us the right direction

        /*if (velocity != Vector3.zero)
        {*/
        Vector3 eular = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eular.x + 90, eular.y + 0, eular.z + 0); // Adjust these numbers to make the boids face different directions!
        //}
    }
}
