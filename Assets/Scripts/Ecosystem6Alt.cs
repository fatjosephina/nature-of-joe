using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem6Alt : MonoBehaviour
{
    public float maxSpeed = 2, maxForce = 2;
    public float sep, coh, ali;

    /*public Mesh coneMesh; // If you want to use your own cone mesh, drop it into the editor here.
    public Mesh cubeMesh;
    public Mesh sphereMesh;*/
    public Mesh[] meshes = new Mesh[3];
    public Color[] colors = new Color[3];

    private List<BoidChildAlt> boids; // Declare a List of Vehicle objects.
    private Vector3 minimumPos = new Vector3(0f, 15f, 0f), maximumPos = new Vector3(50f, 15f, 50f);
    private List<BoidParentAlt> boidParents;

    public GameObject RippleParticles;

    // Start is called before the first frame update
    void Start()
    {
        //Mesh[] meshes = { coneMesh, cubeMesh, sphereMesh };
        //findWindowLimits();
        boidParents = new List<BoidParentAlt>();
        for (int j = 0; j < 30; j++)
        {
            boids = new List<BoidChildAlt>();
            for (int i = 0; i < 3; i++)
            {
                float ranX = Random.Range(0f, 50f);
                float ranY = 15f;
                float ranZ = Random.Range(0f, 50f);
                Mesh mesh = meshes[i];
                Color color = colors[i];
                boids.Add(new BoidChildAlt(new Vector3(ranX, ranY, ranZ), minimumPos, maximumPos, maxSpeed, maxForce, i, mesh, color, RippleParticles));
            }
            float ranXParent = Random.Range(0f, 50f);
            float ranYParent = 15f;
            float ranZParent = Random.Range(0f, 50f);
            boidParents.Add(new BoidParentAlt(new Vector3(ranXParent, ranYParent, ranZParent), minimumPos, maximumPos, maxSpeed, maxForce, boids, "Chapter7Predator"));
        }
        foreach (BoidParentAlt parent in boidParents)
        {
            parent.myVehicle.transform.SetParent(this.gameObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BoidParentAlt v in boidParents)
        {
            v.Flock(boidParents, sep, ali, coh);
            foreach (BoidChildAlt w in v.boidColony)
            {
                w.Flock(v.boidColony, sep, ali, coh);
            }
        }
    }

    public void InstantiateCameraAndParticles(GameObject boidChild, GameObject ripCam, GameObject ripParticles)
    {

    }
}

class BoidParentAlt
{
    public List<BoidChildAlt> boidColony;
    private float maxSpeed, maxForce;
    private Vector3 minPos, maxPos;
    public GameObject myVehicle;
    private Rigidbody rb;

    public Vector3 location
    {
        get { return myVehicle.transform.position; }
        set { myVehicle.transform.position = value; }
    }
    public Vector3 velocity
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    public BoidParentAlt(Vector3 initPos, Vector3 _minPos, Vector3 _maxPos, float _maxSpeed, float _maxForce, List<BoidChildAlt> boids, string tag)
    {
        minPos = _minPos - Vector3.one;
        maxPos = _maxPos + Vector3.one;
        maxSpeed = _maxSpeed;
        maxForce = _maxForce;

        boidColony = boids;

        myVehicle = new GameObject();

        myVehicle.transform.position = new Vector3(initPos.x, initPos.y, initPos.z);

        myVehicle.AddComponent<Rigidbody>();
        rb = myVehicle.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false; // Remember to ignore gravity!
        rb.mass = 100f;

        myVehicle.gameObject.tag = tag;

        foreach (BoidChildAlt boid in boidColony)
        {
            boid.myVehicle.transform.SetParent(myVehicle.transform);
        }
    }

    public void Flock(List<BoidParentAlt> boids, float sepM, float aliM, float cohM)
    {
        Vector3 sep = Separate(boids); // The three flocking rules
        Vector3 ali = Align(boids);
        Vector3 coh = Cohesion(boids);

        sep *= sepM; // Arbitrary weights for these forces (Try different ones!)
        ali *= aliM;
        coh *= cohM;

        ApplyForce(sep); // Applying all the forces
        ApplyForce(ali);
        ApplyForce(coh);
    }

    public Vector3 Align(List<BoidParentAlt> boids)
    {
        float neighborDist = 6f; // This is an arbitrary value and could vary from boid to boid.

        /* Add up all the velocities and divide by the total to
         * calculate the average velocity. */
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (BoidParentAlt other in boids)
        {
            if (other != this)
            {
                float d = Vector3.Distance(location, other.location);
                if ((d > 0) && (d < neighborDist))
                {
                    sum += other.velocity;
                    count++; // For an average, we need to keep track of how many boids are within the distance.
                }
            }
        }

        if (count > 0)
        {
            sum /= count;

            sum = sum.normalized * maxSpeed; // We desite to go in that direction at maximum speed.

            Vector3 steer = sum - velocity; // Reynolds's steering force formula.
            steer = Vector3.ClampMagnitude(steer, maxForce);
            return steer;
        }
        else
        {
            return Vector3.zero; // If we don't find any close boids, the steering force is Zero.
        }
    }

    public Vector3 Cohesion(List<BoidParentAlt> boids)
    {
        float neighborDist = 6f;
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (BoidParentAlt other in boids)
        {
            if (other != this)
            {
                float d = Vector3.Distance(location, other.location);
                if ((d > 0) && (d < neighborDist))
                {
                    sum += other.location; // Adding up all the other's locations
                    count++;
                }
            }
        }
        if (count > 0)
        {
            sum /= count;
            /* Here we make use of the Seek() function we wrote in
             * Example 6.8. The target we seek is thr average
             * location of our neighbors. */
            return Seek(sum);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - location;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steer = desired - velocity;
        steer = Vector3.ClampMagnitude(steer, maxForce);

        return steer;
    }

    public Vector3 Separate(List<BoidParentAlt> boids)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        float desiredSeperation = myVehicle.transform.localScale.x * 2;

        foreach (BoidParentAlt other in boids)
        {
            if (other != this)
            {
                float d = Vector3.Distance(other.location, location);

                if ((d > 0) && (d < desiredSeperation))
                {
                    Vector3 diff = location - other.location;
                    diff.Normalize();

                    diff /= d;

                    sum += diff;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            sum /= count;

            sum *= maxSpeed;

            Vector3 steer = sum - velocity;
            steer = Vector3.ClampMagnitude(steer, maxForce);


            return steer;
        }
        return Vector3.zero;
    }

    public void ApplyForce(Vector3 force)
    {
        rb.AddForce(force);
    }
}

class BoidChildAlt
{
    // To make it easier on ourselves, we use Get and Set as quick ways to get the location of the vehicle
    public Vector3 location
    {
        get { return myVehicle.transform.position; }
        set { myVehicle.transform.position = value; }
    }
    public Vector3 velocity
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    private float maxSpeed, maxForce;
    private Vector3 minPos, maxPos;
    public GameObject myVehicle;
    private Rigidbody rb;

    public BoidChildAlt(Vector3 initPos, Vector3 _minPos, Vector3 _maxPos, float _maxSpeed, float _maxForce, int childType, Mesh coneMesh, Color color, GameObject ripParticles)
    {
        minPos = _minPos /*- Vector3.one*/;
        maxPos = _maxPos /*+ Vector3.one*/;
        maxSpeed = _maxSpeed;
        maxForce = _maxForce;

        myVehicle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer renderer = myVehicle.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = color;
        GameObject.Destroy(myVehicle.GetComponent<BoxCollider>());

        myVehicle.transform.position = new Vector3(initPos.x, initPos.y, initPos.z);

        GameObject myParticles = GameObject.Instantiate(ripParticles, myVehicle.transform.position, Quaternion.identity);
        myParticles.transform.SetParent(myVehicle.transform);

        myVehicle.AddComponent<Rigidbody>();
        rb = myVehicle.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false; // Remember to ignore gravity!


        /* We want to double check if a custom mesh is
         * being used. If not, we will scale a cube up
         * instead ans use that for our boids. */
        if (coneMesh != null)
        {
            MeshFilter filter = myVehicle.GetComponent<MeshFilter>();
            filter.mesh = coneMesh;
            if (childType == 0)
            {
                myVehicle.transform.localScale = new Vector3(1f, 3f, 2f);
            }
            else if (childType == 1)
            {
                myVehicle.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
            else if (childType == 2)
            {
                myVehicle.transform.localScale = new Vector3(0.5f, 0.5f, 2f);
                maxPos = maxPos - new Vector3(0, 0.8f, 0f);
                minPos = minPos - new Vector3(0f, 0.8f, 0f);
            }
        }
        else
        {
            myVehicle.transform.localScale = new Vector3(1f, 2f, 1f);
        }
    }

    private void checkBounds()
    {
        if (location.x > maxPos.x)
        {
            location = new Vector3(minPos.x, location.y, location.z);
        }
        else if (location.x < minPos.x)
        {
            location = new Vector3(maxPos.x, location.y, location.z);
        }
        if (location.y > maxPos.y)
        {
            location = new Vector3(location.x, minPos.y, location.z);
        }
        else if (location.y < minPos.y)
        {
            location = new Vector3(location.x, maxPos.y, location.z);
        }
        if (location.z > maxPos.z)
        {
            location = new Vector3(location.x, location.y, minPos.z);
        }
        else if (location.z < minPos.z)
        {
            location = new Vector3(location.x, location.y, maxPos.z);
        }
    }

    private void lookForward()
    {
        /* We want our boids to face the same direction
         * that they're going. To do that, we take our location
         * and velocity to see where we're heading. */
        Vector3 futureLocation = location + velocity;
        myVehicle.transform.LookAt(futureLocation); // We can use the built in 'LookAt' function to automatically face us the right direction

        /* In the case our model is facing the wrong direction,
         * we can adjust it using Eular Angles. */
        /*if (velocity != Vector3.zero)
        {*/
            Vector3 eular = myVehicle.transform.rotation.eulerAngles;
            myVehicle.transform.rotation = Quaternion.Euler(eular.x + 90, eular.y + 0, eular.z + 0); // Adjust these numbers to make the boids face different directions!
        //}
    }

    public void Flock(List<BoidChildAlt> boids, float sepM, float aliM, float cohM)
    {
        Vector3 sep = Separate(boids); // The three flocking rules
        Vector3 ali = Align(boids);
        Vector3 coh = Cohesion(boids);

        sep *= sepM; // Arbitrary weights for these forces (Try different ones!)
        ali *= aliM;
        coh *= cohM;

        // If the velocity is 0, then the rotation of the boids will glitch. This is one way of preventing this. Another way is making the euler angles only activate if the game object has a velocity larger than 0.
        if (sep == Vector3.zero && ali == Vector3.zero && coh == Vector3.zero)
        {
            sep = new Vector3(0.1f, 0f, 0.1f); ali = new Vector3(0.1f, 0f, 0.1f); coh = new Vector3(0.1f, 0f, 0.1f);
        }

        ApplyForce(sep); // Applying all the forces
        ApplyForce(ali);
        ApplyForce(coh);

        checkBounds(); // To loop the world to the other side of the screen.
        lookForward(); // Make the boids face forward.
    }

    public Vector3 Align(List<BoidChildAlt> boids)
    {
        float neighborDist = 30f; // This is an arbitrary value and could vary from boid to boid.

        /* Add up all the velocities and divide by the total to
         * calculate the average velocity. */
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (BoidChildAlt other in boids)
        {
            if (other != this)
            {
                float d = Vector3.Distance(location, other.location);
                if ((d > 0) && (d < neighborDist))
                {
                    sum += other.velocity;
                    count++; // For an average, we need to keep track of how many boids are within the distance.
                }
            }
        }

        if (count > 0)
        {
            sum /= count;

            sum = sum.normalized * maxSpeed; // We desite to go in that direction at maximum speed.

            Vector3 steer = sum - velocity; // Reynolds's steering force formula.
            steer = Vector3.ClampMagnitude(steer, maxForce);
            return steer;
        }
        else
        {
            return Vector3.zero; // If we don't find any close boids, the steering force is Zero.
        }
    }

    public Vector3 Cohesion(List<BoidChildAlt> boids)
    {
        float neighborDist = 30f;
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (BoidChildAlt other in boids)
        {
            if (other != this)
            {
                float d = Vector3.Distance(location, other.location);
                if ((d > 0) && (d < neighborDist))
                {
                    sum += other.location; // Adding up all the other's locations
                    count++;
                }
            }
        }
        if (count > 0)
        {
            sum /= count;
            /* Here we make use of the Seek() function we wrote in
             * Example 6.8. The target we seek is thr average
             * location of our neighbors. */
            return Seek(sum);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - location;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steer = desired - velocity;
        steer = Vector3.ClampMagnitude(steer, maxForce);

        return steer;
    }

    public Vector3 Separate(List<BoidChildAlt> boids)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        float desiredSeperation = myVehicle.transform.localScale.x * 2;

        foreach (BoidChildAlt other in boids)
        {
            if (other != this)
            {
                float d = Vector3.Distance(other.location, location);

                if ((d > 0) && (d < desiredSeperation))
                {
                    Vector3 diff = location - other.location;
                    diff.Normalize();

                    diff /= d;

                    sum += diff;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            sum /= count;

            sum *= maxSpeed;

            Vector3 steer = sum - velocity;
            steer = Vector3.ClampMagnitude(steer, maxForce);


            return steer;
        }
        return Vector3.zero;
    }

    public void ApplyForce(Vector3 force)
    {
        rb.AddForce(force);
    }
}
