using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorAttract : MonoBehaviour
{
    public float gravityForce = 6.7f;
    public float mass;
    Vector3 location;
    public GameObject predator;
    public string predatorTag = "";
    public bool isAlive = true;

    Ecosystem ecosystem;
    List<GameObject> chapterCreatures;

    // Start is called before the first frame update
    void Start()
    {
        ecosystem = GameObject.Find("Scripts").GetComponent<Ecosystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            GameObject[] predators = GameObject.FindGameObjectsWithTag(predatorTag);
            if (predators.Length > 0)
            {
                foreach (GameObject predator in predators)
                {
                    location = this.gameObject.transform.position;
                    predator.transform.GetComponent<Rigidbody>().AddForce(predator.transform.forward, ForceMode.Acceleration);
                    predator.transform.GetComponent<Rigidbody>().AddForce(Attract(predator), ForceMode.Acceleration);

                    float dist = Vector3.Distance(predator.transform.position, location);
                    if (dist <= 4f)
                    {
                        isAlive = false;
                        if (predatorTag == "Chapter1Predator")
                        {
                            ecosystem.chapter1Creatures.Remove(this.gameObject);
                        }
                        else if (predatorTag == "Chapter2Predator")
                        {
                            ecosystem.chapter2Creatures.Remove(this.gameObject);
                        }
                        else if (predatorTag == "Chapter3Predator")
                        {
                            ecosystem.chapter3Creatures.Remove(this.gameObject);
                        }
                        else if (predatorTag == "Chapter6Predator")
                        {
                            ecosystem.chapter6Creatures.Remove(this.gameObject);
                        }
                        else if (predatorTag == "Chapter7Predator")
                        {
                            ecosystem.chapter7Creatures.Remove(this.gameObject);
                        }
                        else if (predatorTag == "Chapter8Predator")
                        {
                            ecosystem.chapter8Creatures.Remove(this.gameObject);
                        }
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public Vector3 Attract(GameObject predator)
    {
        Vector3 difference = location - predator.transform.position;
        float dist = difference.magnitude;
        Vector3 gravityDirection = difference.normalized;
        float gravity = gravityForce * (mass * predator.GetComponent<Rigidbody>().mass) / (dist * dist);
        Vector3 gravityVector = gravityDirection * gravity;

        return gravityVector;
    }
}
