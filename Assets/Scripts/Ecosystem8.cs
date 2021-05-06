﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Ecosystem8 : MonoBehaviour
{
    public Material branchMaterial;

    private Chapter8Fig10LSystem lSys;
    private Vector2 screenSize;
    private float len;
    private float theta;
    private LineRenderer lineRenderer;
    private Chapter8Fig10LSystemState state;

    // A stack is a collection of objects that functions like a list in that the size is dynamic. You can do one of two things with a stack: 
    // Place an object on top of the stack (Push) Or take the object off the top of the stack and assign/manipulate it. 
    // Think of it like a stack of plates. You can either place a plate on top of the stack or take one from the top.
    private Stack<Chapter8Fig10LSystemState> savedStates;
    private List<GameObject> lines;

    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        lines = new List<GameObject>();
        savedStates = new Stack<Chapter8Fig10LSystemState>();
        state = new Chapter8Fig10LSystemState();
        counter = 0;

        // Start drawing lines by modifying the GO's location to be bottom center
        transform.position = this.gameObject.transform.position + new Vector3(0f, 5f, 0f);

        Chapter8Fig10Rule[] ruleset = new Chapter8Fig10Rule[1];
        ruleset[0] = new Chapter8Fig10Rule('F', "FF+[+F-F-F]-[-F+F+F]");
        lSys = new Chapter8Fig10LSystem("F", ruleset);

        len = Random.Range(0.1f, 5f);

        // Degress of rotation
        theta = 45;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = branchMaterial;

        // Set both the start and end width to 0.05
        lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;
        savedStates.Push(state.Clone());
        lSys.Generate();
        state = savedStates.Pop();
        drawLines();
    }

    private void drawLines()
    {
        // Assiging values to our new state's values while instantiating
        state = new Chapter8Fig10LSystemState()
        {
            X = 0,
            Y = 0,
            Size = len,
            Angle = 0
        };

        string sentence = lSys.Sentence;
        for (int i = 0; i < sentence.Length; i++)
        {
            char c = sentence[i];
            if (c == 'F' || c == 'G')
            {
                line();
            }
            else if (c == '+')
            {
                state.Angle += theta;
            }
            else if (c == '-')
            {
                state.Angle -= theta;
            }
            else if (c == '[')
            {
                savedStates.Push(state.Clone());
            }
            else if (c == ']')
            {
                state = savedStates.Pop();
            }
        }
    }

    private void line()
    {
        var lineGO = new GameObject();
        lineGO.transform.position = Vector3.zero;
        lineGO.transform.parent = transform;

        lines.Add(lineGO);
        LineRenderer newLine = setupLine(lineGO);
        newLine.SetPosition(0, new Vector3(state.X + transform.position.x, state.Y + transform.position.y, transform.position.z));
        checkAngles();
        newLine.SetPosition(1, new Vector3(state.X + transform.position.x, state.Y + transform.position.y, transform.position.z));
    }

    private void checkAngles()
    {
        if (state.Angle != 0)
        {
            state.X += Mathf.Sin(state.Angle / 100);
            state.Y += Mathf.Cos(state.Angle / 100);
        }
        else
        {
            state.Y += state.Size;
        }
    }

    private LineRenderer setupLine(GameObject lineGO)
    {
        var newLine = lineGO.AddComponent<LineRenderer>();
        newLine.useWorldSpace = true;
        newLine.positionCount = 2;
        newLine.material = lineRenderer.material;
        newLine.startColor = lineRenderer.startColor;
        newLine.endColor = lineRenderer.endColor;
        newLine.startWidth = lineRenderer.startWidth;
        newLine.endWidth = lineRenderer.endWidth;
        return newLine;
    }

    // Update is called once per frame
    void Update()
    {
            if (counter < 2)
            {
                lSys.Generate();
                len *= 0.1f;
                drawLines();
                counter++;
            }
    }
}

// Our state of the current L system.
public class Chapter8Fig10LSystemState
{
    public float Size;
    public float Angle;
    public float X;
    public float Y;

    public Chapter8Fig10LSystemState Clone()
    {
        // Memeberwise clone performs a shallow copy of an object and returns it. The nonstatic fields get transferred over to the new object.
        // This returns a base object which we can cast into a system state object
        // Documentation: https://docs.microsoft.com/en-us/dotnet/api/system.object.memberwiseclone?view=netcore-3.1
        return (Chapter8Fig10LSystemState)MemberwiseClone();
    }
}

public class Chapter8Fig10LSystem
{
    public string Sentence { get; private set; }
    private Chapter8Fig10Rule[] ruleset;
    public int Generation { get; private set; }

    public Chapter8Fig10LSystem(string axiom, Chapter8Fig10Rule[] r)
    {
        Sentence = axiom;
        ruleset = r;
        Generation = 0;
    }

    public void Generate()
    {
        char[] sentenceCharArray = Sentence.ToCharArray();
        System.Text.StringBuilder nextGen = new StringBuilder();
        for (int i = 0; i < Sentence.Length; i++)
        {
            char curr = sentenceCharArray[i];
            string replace = "" + curr;
            for (int j = 0; j < ruleset.Length; j++)
            {
                char a = ruleset[j].A;
                if (a == curr)
                {
                    replace = ruleset[j].B;
                    break;
                }
            }
            nextGen.Append(replace);
        }
        Sentence = nextGen.ToString();
        Generation++;
    }

}

public class Chapter8Fig10Rule
{
    public char A { get; private set; }
    public string B { get; private set; }

    public Chapter8Fig10Rule(char _a, string _b)
    {
        A = _a;
        B = _b;
    }
}
