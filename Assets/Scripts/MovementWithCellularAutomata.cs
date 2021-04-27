using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementWithCellularAutomata : MonoBehaviour
{
    // A list to store ruleset arrays
    public List<int[]> rulesetList = new List<int[]>();

    // Custom Rulesets
    public int[] ruleSet0 = { 0, 1, 0, 1, 1, 0, 1, 0 };
    public int[] ruleSet1 = { 1, 0, 1, 0, 1, 0, 1, 0 };
    public int[] ruleSet2 = { 0, 1, 0, 1, 1, 0, 1, 0 };
    public int[] ruleSet3 = { 1, 1, 0, 0, 1, 0, 1, 1 };
    public int[] ruleSet4 = { 0, 0, 0, 1, 1, 0, 1, 0 };
    public int[] ruleSet5 = { 1, 0, 0, 1, 1, 0, 1, 0 };
    public int[] ruleSet6 = { 0, 1, 1, 0, 1, 0, 1, 1 };

    private int rulesChosen;

    // An object to describe a Wolfram elementary Cellular Automata
    Chapter7Fig1CA ca;

    // How long after the CA has been drawn before reloading the scene, choosing new rule
    private int delay = 0;

    Creature7Body body;

    // Start is called before the first frame update
    void Start()
    {
        addRuleSetsToList();

        // Choosing a random rule set using Random.Range
        rulesChosen = Random.Range(0, rulesetList.Count);
        int[] ruleset = rulesetList[rulesChosen];
        ca = new Chapter7Fig1CA(ruleset); // Initialize CA

        limitFrameRate();
       // setOrthographicCamera();
        body = new Creature7Body();
        StartCoroutine(TimeManager());
    }

    private void FixedUpdate()
    {
       
    }

    IEnumerator ChangePath()
    {
        yield return new WaitForSeconds(3f);
        ca.Randomize();
        ca.restart();
        ca.Generate();
        ca.Display(body); // Draw the CA
        StartCoroutine(TimeManager());
    }

    IEnumerator TimeManager()
    {
        Debug.Log("Changing");
        yield return new WaitForSeconds(2f);
        StartCoroutine(ChangePath());
    }

    private void addRuleSetsToList()
    {
        rulesetList.Add(ruleSet0);
        rulesetList.Add(ruleSet1);
        rulesetList.Add(ruleSet2);
        rulesetList.Add(ruleSet3);
        rulesetList.Add(ruleSet4);
        rulesetList.Add(ruleSet5);
        rulesetList.Add(ruleSet6);
    }

    private void setOrthographicCamera()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
    }

    private void limitFrameRate()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
    }
}

public class Chapter7Fig1CA
{
    private int[] cells; // An array of 0s and 1s
    private int generation; // How many generations?
    private int[] ruleset; // An array to store the ruleset, for example {0,1,1,0,1,1,0,1}
    private int rowWidth; // How wide to make the array
    private int cellCapacity; // We limit how many cells we instantiate
    private int numberOfCells; // Which needs us to keep count

    public Chapter7Fig1CA(int[] ruleSetToUse)
    {
        rowWidth = 17;
        cellCapacity = 650;

        // How big our screen is in World Units
        numberOfCells = 0;
        ruleset = ruleSetToUse;
        cells = new int[cellCapacity / rowWidth];
        restart();
    }

    public void Randomize() // If we wanted to make a random Ruleset
    {
        for (int i = 0; i < 8; i++)
        {
            ruleset[i] = Random.Range(0, 2);
        }
    }

    public void restart()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = 0;
        }
        cells[cells.Length / 2] = 1; // We arbitrarily start with just the middle cell having a state of "1"
        generation = 0;
    }

    // The process of creating the new generation
    public void Generate()
    {
        // First we create an empty array for the new values
        int[] nextGen = new int[cells.Length];

        // For every spot, determine new state by examing current state, and neighbor states
        // Ignore edges that only have one neighor
        for (int i = 1; i < cells.Length - 1; i++)
        {
            int left = cells[i - 1]; // Left neighbor state
            int me = cells[i]; // Current state
            int right = cells[i + 1]; // Right neighbor state
            nextGen[i] = rules(left, me, right); // Compute next generation state based on ruleset
        }

        // The current generation is the new generation
        cells = nextGen;
        generation++;
    }

    public void Display(Creature7Body gameObject) // Drawing the cells. Cells with a state of 1 are black, cells with a state of 0 are white
    {
        if (numberOfCells <= cellCapacity)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                numberOfCells++;
                if (cells[i] == 1)
                {
                    //gameObject.Movement1();
                    gameObject.location.x += 0.3f;
                    gameObject.location.y -= 0.3f;
                    //gameObject.mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    gameObject.mover.transform.position = gameObject.location;
                }
                else
                {
                    //gameObject.location.x -= 0.1f;
                    gameObject.location.y += 0.3f;
                    //gameObject.mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    gameObject.mover.transform.position = gameObject.location;
                }
                // Set position based to lower left of screen, with screen offset
            }
        }
    }

    private int rules(int a, int b, int c) // Implementing the Wolfram rules
    {
        if (a == 1 && b == 1 && c == 1) return ruleset[0];
        if (a == 1 && b == 1 && c == 0) return ruleset[1];
        if (a == 1 && b == 0 && c == 1) return ruleset[2];
        if (a == 1 && b == 0 && c == 0) return ruleset[3];
        if (a == 0 && b == 1 && c == 1) return ruleset[4];
        if (a == 0 && b == 1 && c == 0) return ruleset[5];
        if (a == 0 && b == 0 && c == 1) return ruleset[6];
        if (a == 0 && b == 0 && c == 0) return ruleset[7];
        return 0;
    }
}

public class Creature7Body
{
    public Vector3 location, velocity, acceleration;
    private float topSpeed;

    private Vector2 minimumPos, maximumPos;

    public GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Creature7Body()
    {
        location = Vector3.zero; // Vector2.zero is a (0, 0) vector
        velocity = Vector3.zero;
        acceleration = new Vector3(-0.1F, 0f, -0.1F);
        topSpeed = 5F;
    }
}
