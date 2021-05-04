using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reproduce : MonoBehaviour
{
    public string predatorTag = "";
    Vector3 location;
    private bool canReproduce = false;
    public GameObject chapter1Creature;
    public GameObject chapter2Creature;
    public GameObject chapter7Creature;

    private void Start()
    {
        StartCoroutine(ReproductionSwitch());
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<PredatorAttract>().isAlive)
        {
            if (canReproduce)
            {
                GameObject[] predators = GameObject.FindGameObjectsWithTag(predatorTag);
                foreach (GameObject predator in predators)
                {
                    location = this.gameObject.transform.position;
                    float dist = Vector3.Distance(predator.transform.position, location);
                    if (dist <= 1f)
                    {
                        Debug.Log(predators.Length);
                        if (predators.Length < 15)
                        {
                            if (predatorTag == "Chapter2Predator")
                            {
                                GameObject c = Instantiate(chapter1Creature, location, Quaternion.identity);
                                Ecosystem ecosystem = GameObject.Find("Scripts").GetComponent<Ecosystem>();
                                ecosystem.chapter1Creatures.Add(c);
                            }
                            if (predatorTag == "FoodPredator")
                            {
                                GameObject c = Instantiate(chapter2Creature, location, Quaternion.identity);
                                Ecosystem ecosystem = GameObject.Find("Scripts").GetComponent<Ecosystem>();
                                ecosystem.chapter2Creatures.Add(c);
                            }
                            if (predatorTag == "WaterPredator")
                            {
                                GameObject c = Instantiate(chapter7Creature, location, Quaternion.identity);
                                Ecosystem ecosystem = GameObject.Find("Scripts").GetComponent<Ecosystem>();
                                ecosystem.chapter7Creatures.Add(c);
                            }
                            canReproduce = false;
                            StartCoroutine(ReproductionSwitch());
                        }
                    }
                }
            }
        }
    }

    private IEnumerator ReproductionSwitch()
    {
        yield return new WaitForSeconds(10f);
        canReproduce = true;
    }
}
