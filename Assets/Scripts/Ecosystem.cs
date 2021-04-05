using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the script that is going to control the entire ecosystem
public class Ecosystem : MonoBehaviour
{
    public List<GameObject> chapter1Creatures = new List<GameObject>();
    public GameObject chapter1Creature;
    public int chapter1CreaturePopulation;

    public List<GameObject> chapter2Creatures = new List<GameObject>();
    public GameObject chapter2Creature;
    public int chapter2CreaturePopulation;

    public List<GameObject> chapter3Creatures = new List<GameObject>();
    public GameObject chapter3Creature;
    public int chapter3CreaturePopulation;

    public List<GameObject> chapter6Creatures = new List<GameObject>();
    public GameObject chapter6Creature;
    public int chapter6CreaturePopulation;

    public List<GameObject> chapter7Creatures = new List<GameObject>();
    public GameObject chapter7Creature;
    public int chapter7CreaturePopulation;

    public List<GameObject> chapter8Creatures = new List<GameObject>();
    public GameObject chapter8Creature;
    public int chapter8CreaturePopulation;

    public PerlinTerrain terrain;
    public float terrainMin;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < chapter1CreaturePopulation; i++)
        {
            chapter1Creature = Instantiate(chapter1Creature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapter1Creatures.Add(chapter1Creature);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
