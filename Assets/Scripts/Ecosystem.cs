using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the script that is going to control the entire ecosystem
public class Ecosystem : MonoBehaviour
{
    public List<GameObject> chapter1Creatures = new List<GameObject>();
    public GameObject chapter1Creature;
    public int chapter1CreaturePopulation;
    public int chapter1MinimumPopulation;

    public List<GameObject> chapter2Creatures = new List<GameObject>();
    public GameObject chapter2Creature;
    public int chapter2CreaturePopulation;
    public int chapter2MinimumPopulation;

    public List<GameObject> chapter3Creatures = new List<GameObject>();
    public GameObject chapter3Creature;
    public int chapter3CreaturePopulation;
    public int chapter3MinimumPopulation;

    public List<GameObject> chapter6Creatures = new List<GameObject>();
    public GameObject chapter6Creature;
    public int chapter6CreaturePopulation;
    public int chapter6MinimumPopulation;

    public List<GameObject> chapter7Creatures = new List<GameObject>();
    public GameObject chapter7Creature;
    public int chapter7CreaturePopulation;
    public int chapter7MinimumPopulation;

    public List<GameObject> chapter8Creatures = new List<GameObject>();
    public GameObject chapter8Creature;
    public int chapter8CreaturePopulation;
    public int chapter8MinimumPopulation;

    public PerlinTerrain terrain;
    public float terrainMin;

    //Animal food
    public GameObject flower;
    public int flowerTotal;
    public int flowerMin;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < chapter1CreaturePopulation; i++)
        {
            GameObject chapter1C = Instantiate(chapter1Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter1Creatures.Add(chapter1C);
        }
        for (int i = 0; i < chapter2CreaturePopulation; i++)
        {
            GameObject chapter2C = Instantiate(chapter2Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter2Creatures.Add(chapter2C);
        }
        for (int i = 0; i < chapter3CreaturePopulation; i++)
        {
            GameObject chapter3C = Instantiate(chapter3Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter3Creatures.Add(chapter3C);
        }
        for (int i = 0; i < chapter6CreaturePopulation; i++)
        {
            GameObject chapter6C = Instantiate(chapter6Creature, Vector3.zero, Quaternion.identity);
            chapter6Creatures.Add(chapter6C);
        }
        for (int i = 0; i < chapter7CreaturePopulation; i++)
        {
            GameObject chapter7C = Instantiate(chapter7Creature, new Vector3(Random.Range(terrainMin, terrain.cols), 15.25f, Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter7Creatures.Add(chapter7C);
        }
        for (int i = 0; i < chapter8CreaturePopulation; i++)
        {
            GameObject chapter8C = Instantiate(chapter8Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter8Creatures.Add(chapter8C);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (chapter1Creatures.Count <= chapter1MinimumPopulation)
        {
            GameObject c = Instantiate(chapter1Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter1Creatures.Add(c);
        }
        if (chapter2Creatures.Count <= chapter2MinimumPopulation)
        {
            GameObject c = Instantiate(chapter2Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter2Creatures.Add(c);
        }
        if (chapter3Creatures.Count <= chapter3MinimumPopulation)
        {
            GameObject c = Instantiate(chapter3Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter3Creatures.Add(c);
        }
        if (chapter6Creatures.Count <= chapter6MinimumPopulation)
        {
            GameObject c = Instantiate(chapter6Creature, Vector3.zero, Quaternion.identity);
            chapter6Creatures.Add(c);
        }
        if (chapter7Creatures.Count <= chapter7MinimumPopulation)
        {
            GameObject c = Instantiate(chapter7Creature, new Vector3(Random.Range(terrainMin, terrain.cols), 15.25f, Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter7Creatures.Add(c);
        }
        if (chapter8Creatures.Count <= chapter8MinimumPopulation)
        {
            GameObject c = Instantiate(chapter8Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter8Creatures.Add(c);
        }
    }

    IEnumerator circleOfLife(GameObject prey, Vector3 position)
    {
        yield return new WaitForSeconds(10);
        if (prey.name == chapter1Creature.name || prey.name == chapter1Creature.name + "(Clone)")
        {
            GameObject c = Instantiate(prey, position, Quaternion.identity);
            chapter1Creatures.Add(c);
        }
        yield return new WaitForSeconds(10);
        if (prey.name == chapter2Creature.name || prey.name == chapter2Creature.name + "(Clone)")
        {
            GameObject c = Instantiate(prey, position, Quaternion.identity);
            chapter2Creatures.Add(c);
        }
        yield return new WaitForSeconds(10);
        if (prey.name == chapter3Creature.name || prey.name == chapter3Creature.name + "(Clone)")
        {
            GameObject c = Instantiate(prey, position, Quaternion.identity);
            chapter3Creatures.Add(c);
        }
        yield return new WaitForSeconds(10);
        if (prey.name == chapter6Creature.name || prey.name == chapter6Creature.name + "(Clone)")
        {
            GameObject c = Instantiate(prey, position, Quaternion.identity);
            chapter6Creatures.Add(c);
        }
        yield return new WaitForSeconds(10);

        if (prey.name == chapter7Creature.name || prey.name == chapter7Creature.name + "(Clone)")
        {
            GameObject c = Instantiate(prey, position, Quaternion.identity);
            chapter7Creatures.Add(c);
        }
        yield return new WaitForSeconds(10);

        if (prey.name == chapter8Creature.name || prey.name == chapter8Creature.name + "(Clone)")
        {
            GameObject c = Instantiate(prey, position, Quaternion.identity);
            chapter8Creatures.Add(c);
        }
    }
}
