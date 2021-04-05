using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Eric, Dat, Joseph, Gabriel
public class Chapter3E1 : MonoBehaviour
{
    public GameObject Baton;

    public Vector3 aVelocity = new Vector3(0f, 0f, 0f);
    public Vector3 aAcceleration = new Vector3(0f, 0f, .001f);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        aVelocity += aAcceleration;
        Baton.transform.Rotate(aVelocity, Space.World);
    }
}
