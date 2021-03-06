using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem6 : MonoBehaviour
{
    //public float maxSpeed = 2, maxForce = 2;
    public float sep, coh, ali;

    /*public Mesh coneMesh; // If you want to use your own cone mesh, drop it into the editor here.
    public Mesh cubeMesh;
    public Mesh sphereMesh;*/
    public Mesh[] meshes = new Mesh[3];

    private Vector3 minimumPos, maximumPos;
    public List<BoidParent> boidParents = new List<BoidParent>();

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

   // private 
    private List<List<BoidChild>> boidChildren = new List<List<BoidChild>>();
    private float maxSpeed = 2, maxForce = 2;
    private Vector3 minPos, maxPos;
    public GameObject myVehicle;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //Mesh[] meshes = { coneMesh, cubeMesh, sphereMesh };
        /*findWindowLimits();
        boidParents = new List<BoidParent>();
        for (int j = 0; j < 30; j++)
        {
            boids = new List<BoidChild>();
            for (int i = 0; i < 3; i++)
            {
                float ranX = Random.Range(-1.0f, 1.0f);
                float ranY = Random.Range(-1.0f, 1.0f);
                Mesh mesh = meshes[i];
                boids.Add(new BoidChild(new Vector3(ranX, ranY), minimumPos, maximumPos, maxSpeed, maxForce, mesh));
            }
            float ranXParent = Random.Range(-1.0f, 1.0f);
            float ranYParent = Random.Range(-1.0f, 1.0f);
            boidParents.Add(new BoidParent(new Vector3(ranXParent, ranYParent), minimumPos, maximumPos, maxSpeed, maxForce, boids));
        }*/

        maximumPos = new Vector3(50f, 10f, 50f);
        minimumPos = new Vector3(0f, 2f, 0f);


            for (int j = 0; j < 10; j++)
            {

                List<BoidChild> boidColony = new List<BoidChild>();

                for (int i = 0; i < 3; i++)
                {
                float ranX = Random.Range(0f, 10.0f);
                float ranY = Random.Range(0f, 10.0f);
                float ranZ = Random.Range(0f, 10.0f);
                Mesh mesh = meshes[Random.Range(0,2)];
                boidColony.Add(new BoidChild(new Vector3(ranX, ranY, ranZ), minimumPos, maximumPos, maxSpeed, maxForce, mesh));
                boidChildren.Add(boidColony);

                float randoX = Random.Range(0f, 20.0f);
                    float randoY = Random.Range(0f, 20.0f);
                    float randoZ = Random.Range(0f, 20.0f);
                    boidParents.Add(new BoidParent(new Vector3(randoX, randoY, randoZ), minimumPos, maximumPos, maxSpeed, maxForce, boidChildren[j]));
                }



        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 mousePos = Input.mousePosition;
        //mousePos = Camera.main.ScreenToWorldPoint(mousePos);


        for(int i = 0; i < boidChildren.Count; i++)
        {
            foreach (BoidChild w in boidChildren[i])
            {
                w.Flock(boidChildren[i], sep, ali, coh);
            }
        }



        foreach (BoidParent v in boidParents)
        {
            v.Flock(boidParents, sep, ali, coh);
        }

        //foreach (Ecosystem6 v in boidParents)
        //{
        //    v.Flock(boidParents, sep, ali, coh);
        //    foreach (BoidChild w in v.boidColony)
        //    {
        //        w.Flock(v.boidColony, sep, ali, coh);
        //    }
        //}
    }


    public class BoidParent
    {
        public List<BoidChild> boidColony;
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

        public BoidParent(Vector3 initPos, Vector3 _minPos, Vector3 _maxPos, float _maxSpeed, float _maxForce, List<BoidChild> boids)
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

            foreach (BoidChild boid in boidColony)
            {
                boid.myVehicle.transform.SetParent(myVehicle.transform);
            }
        }

        public void Flock(List<BoidParent> boids, float sepM, float aliM, float cohM)
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

            checkBounds(); // To loop the world to the other side of the screen.
            lookForward(); // Make the boids face forward.
        }

        public Vector3 Align(List<BoidParent> boids)
        {
            float neighborDist = 6f; // This is an arbitrary value and could vary from boid to boid.

            /* Add up all the velocities and divide by the total to
             * calculate the average velocity. */
            Vector3 sum = Vector3.zero;
            int count = 0;
            foreach (BoidParent other in boids)
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

        public Vector3 Cohesion(List<BoidParent> boids)
        {
            float neighborDist = 6f;
            Vector3 sum = Vector3.zero;
            int count = 0;
            foreach (BoidParent other in boids)
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

        public Vector3 Separate(List<BoidParent> boids)
        {
            Vector3 sum = Vector3.zero;
            int count = 0;

            float desiredSeperation = myVehicle.transform.localScale.x * 2;

            foreach (BoidParent other in boids)
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

        private void checkBounds()
        {
            if (location.x > maxPos.x)
            {
                location = new Vector3(minPos.x, location.y);
            }
            else if (location.x < minPos.x)
            {
                location = new Vector3(maxPos.x, location.y);
            }
            if (location.y > maxPos.y)
            {
                location = new Vector3(location.x, minPos.y);
            }
            else if (location.y < minPos.y)
            {
                location = new Vector3(location.x, maxPos.y);
            }
            if(location.z > maxPos.z)
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
            Vector3 eular = myVehicle.transform.rotation.eulerAngles;
            myVehicle.transform.rotation = Quaternion.Euler(eular.x + 90, eular.y + 0, eular.z + 0); // Adjust these numbers to make the boids face different directions!
        }
    }

    public class BoidChild
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
        public Rigidbody rb;

        public BoidChild(Vector3 initPos, Vector3 _minPos, Vector3 _maxPos, float _maxSpeed, float _maxForce, Mesh coneMesh)
        {
            minPos = _minPos - Vector3.one;
            maxPos = _maxPos + Vector3.one;
            maxSpeed = _maxSpeed;
            maxForce = _maxForce;

            myVehicle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Renderer renderer = myVehicle.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Diffuse"));
            renderer.material.color = Color.red;
            GameObject.Destroy(myVehicle.GetComponent<BoxCollider>());

            myVehicle.transform.position = new Vector3(initPos.x, initPos.y, initPos.z);

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
                location = new Vector3(minPos.x, location.y);
            }
            else if (location.x < minPos.x)
            {
                location = new Vector3(maxPos.x, location.y);
            }
            if (location.y > maxPos.y)
            {
                location = new Vector3(location.x, minPos.y);
            }
            else if (location.y < minPos.y)
            {
                location = new Vector3(location.x, maxPos.y);
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
            location = myVehicle.transform.position;
            /* We want our boids to face the same direction
             * that they're going. To do that, we take our location
             * and velocity to see where we're heading. */
            Vector3 futureLocation = location + velocity;
            myVehicle.transform.LookAt(futureLocation); // We can use the built in 'LookAt' function to automatically face us the right direction

            /* In the case our model is facing the wrong direction,
             * we can adjust it using Eular Angles. */
            Vector3 eular = myVehicle.transform.rotation.eulerAngles;
            myVehicle.transform.rotation = Quaternion.Euler(eular.x + 90, eular.y + 0, eular.z + 0); // Adjust these numbers to make the boids face different directions!
        }

        public void Flock(List<BoidChild> boids, float sepM, float aliM, float cohM)
        {
            lookForward();
            Vector3 sep = Separate(boids); // The three flocking rules
            Vector3 ali = Align(boids);
            Vector3 coh = Cohesion(boids);

            sep *= sepM; // Arbitrary weights for these forces (Try different ones!)
            ali *= aliM;
            coh *= cohM;

            ApplyForce(sep); // Applying all the forces
            ApplyForce(ali);
            ApplyForce(coh);

            checkBounds(); // To loop the world to the other side of the screen.
            //lookForward(); // Make the boids face forward.
        }

        public Vector3 Align(List<BoidChild> boids)
        {
            float neighborDist = 30f; // This is an arbitrary value and could vary from boid to boid.

            /* Add up all the velocities and divide by the total to
             * calculate the average velocity. */
            Vector3 sum = Vector3.zero;
            int count = 0;
            foreach (BoidChild other in boids)
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

        public Vector3 Cohesion(List<BoidChild> boids)
        {
            float neighborDist = 30f;
            Vector3 sum = Vector3.zero;
            int count = 0;
            foreach (BoidChild other in boids)
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

        public Vector3 Separate(List<BoidChild> boids)
        {
            Vector3 sum = Vector3.zero;
            int count = 0;

            float desiredSeperation = myVehicle.transform.localScale.x * 2;

            foreach (BoidChild other in boids)
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
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Ecosystem6 : MonoBehaviour
//{
//    public float maxSpeed = 2, maxForce = 2;
//    public float sep, coh, ali;

//    /*public Mesh coneMesh; // If you want to use your own cone mesh, drop it into the editor here.
//    public Mesh cubeMesh;
//    public Mesh sphereMesh;*/
//    public Mesh[] meshes = new Mesh[3];

//    private List<BoidChild> boids; // Declare a List of Vehicle objects.
//    private Vector3 minimumPos, maximumPos;
//    private List<BoidParent> boidParents;

//    // Start is called before the first frame update
//    void Start()
//    {
//        //Mesh[] meshes = { coneMesh, cubeMesh, sphereMesh };
//        findWindowLimits();
//        boidParents = new List<BoidParent>();
//        for (int j = 0; j < 30; j++)
//        {
//            boids = new List<BoidChild>();
//            for (int i = 0; i < 3; i++)
//            {
//                float ranX = Random.Range(-1.0f, 1.0f);
//                float ranY = Random.Range(-1.0f, 1.0f);
//                Mesh mesh = meshes[i];
//                boids.Add(new BoidChild(new Vector3(ranX, ranY), minimumPos, maximumPos, maxSpeed, maxForce, mesh));
//            }
//            float ranXParent = Random.Range(-1.0f, 1.0f);
//            float ranYParent = Random.Range(-1.0f, 1.0f);
//            boidParents.Add(new BoidParent(new Vector3(ranXParent, ranYParent), minimumPos, maximumPos, maxSpeed, maxForce, boids));
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Vector3 mousePos = Input.mousePosition;
//        mousePos = Camera.main.ScreenToWorldPoint(mousePos);


//        foreach (BoidParent v in boidParents)
//        {
//            v.Flock(boidParents, sep, ali, coh);
//            foreach (BoidChild w in v.boidColony)
//            {
//                w.Flock(v.boidColony, sep, ali, coh);
//            }
//        }
//    }

//    private void findWindowLimits()
//    {
//        Camera.main.orthographic = true;
//        Camera.main.orthographicSize = 20;
//        minimumPos = Camera.main.ScreenToWorldPoint(Vector3.zero);
//        maximumPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
//    }
//}

//class BoidParent
//{
//    public List<BoidChild> boidColony;
//    private float maxSpeed, maxForce;
//    private Vector3 minPos, maxPos;
//    public GameObject myVehicle;
//    private Rigidbody rb;

//    public Vector3 location
//    {
//        get { return myVehicle.transform.position; }
//        set { myVehicle.transform.position = value; }
//    }
//    public Vector3 velocity
//    {
//        get { return rb.velocity; }
//        set { rb.velocity = value; }
//    }

//    public BoidParent(Vector3 initPos, Vector3 _minPos, Vector3 _maxPos, float _maxSpeed, float _maxForce, List<BoidChild> boids)
//    {
//        minPos = _minPos - Vector3.one;
//        maxPos = _maxPos + Vector3.one;
//        maxSpeed = _maxSpeed;
//        maxForce = _maxForce;

//        boidColony = boids;

//        myVehicle = new GameObject();

//        myVehicle.transform.position = new Vector3(initPos.x, initPos.y);

//        myVehicle.AddComponent<Rigidbody>();
//        rb = myVehicle.GetComponent<Rigidbody>();
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.useGravity = false; // Remember to ignore gravity!
//        rb.mass = 100f;

//        foreach (BoidChild boid in boidColony)
//        {
//            boid.myVehicle.transform.SetParent(myVehicle.transform);
//        }
//    }

//    public void Flock(List<BoidParent> boids, float sepM, float aliM, float cohM)
//    {
//        Vector3 sep = Separate(boids); // The three flocking rules
//        Vector3 ali = Align(boids);
//        Vector3 coh = Cohesion(boids);

//        sep *= sepM; // Arbitrary weights for these forces (Try different ones!)
//        ali *= aliM;
//        coh *= cohM;

//        ApplyForce(sep); // Applying all the forces
//        ApplyForce(ali);
//        ApplyForce(coh);

//        checkBounds(); // To loop the world to the other side of the screen.
//        lookForward(); // Make the boids face forward.
//    }

//    public Vector3 Align(List<BoidParent> boids)
//    {
//        float neighborDist = 6f; // This is an arbitrary value and could vary from boid to boid.

//        /* Add up all the velocities and divide by the total to
//         * calculate the average velocity. */
//        Vector3 sum = Vector3.zero;
//        int count = 0;
//        foreach (BoidParent other in boids)
//        {
//            if (other != this)
//            {
//                float d = Vector3.Distance(location, other.location);
//                if ((d > 0) && (d < neighborDist))
//                {
//                    sum += other.velocity;
//                    count++; // For an average, we need to keep track of how many boids are within the distance.
//                }
//            }
//        }

//        if (count > 0)
//        {
//            sum /= count;

//            sum = sum.normalized * maxSpeed; // We desite to go in that direction at maximum speed.

//            Vector3 steer = sum - velocity; // Reynolds's steering force formula.
//            steer = Vector3.ClampMagnitude(steer, maxForce);
//            return steer;
//        }
//        else
//        {
//            return Vector3.zero; // If we don't find any close boids, the steering force is Zero.
//        }
//    }

//    public Vector3 Cohesion(List<BoidParent> boids)
//    {
//        float neighborDist = 6f;
//        Vector3 sum = Vector3.zero;
//        int count = 0;
//        foreach (BoidParent other in boids)
//        {
//            if (other != this)
//            {
//                float d = Vector3.Distance(location, other.location);
//                if ((d > 0) && (d < neighborDist))
//                {
//                    sum += other.location; // Adding up all the other's locations
//                    count++;
//                }
//            }
//        }
//        if (count > 0)
//        {
//            sum /= count;
//            /* Here we make use of the Seek() function we wrote in
//             * Example 6.8. The target we seek is thr average
//             * location of our neighbors. */
//            return Seek(sum);
//        }
//        else
//        {
//            return Vector3.zero;
//        }
//    }

//    public Vector3 Seek(Vector3 target)
//    {
//        Vector3 desired = target - location;
//        desired.Normalize();
//        desired *= maxSpeed;
//        Vector3 steer = desired - velocity;
//        steer = Vector3.ClampMagnitude(steer, maxForce);

//        return steer;
//    }

//    public Vector3 Separate(List<BoidParent> boids)
//    {
//        Vector3 sum = Vector3.zero;
//        int count = 0;

//        float desiredSeperation = myVehicle.transform.localScale.x * 2;

//        foreach (BoidParent other in boids)
//        {
//            if (other != this)
//            {
//                float d = Vector3.Distance(other.location, location);

//                if ((d > 0) && (d < desiredSeperation))
//                {
//                    Vector3 diff = location - other.location;
//                    diff.Normalize();

//                    diff /= d;

//                    sum += diff;
//                    count++;
//                }
//            }
//        }

//        if (count > 0)
//        {
//            sum /= count;

//            sum *= maxSpeed;

//            Vector3 steer = sum - velocity;
//            steer = Vector3.ClampMagnitude(steer, maxForce);


//            return steer;
//        }
//        return Vector3.zero;
//    }

//    public void ApplyForce(Vector3 force)
//    {
//        rb.AddForce(force);
//    }

//    private void checkBounds()
//    {
//        if (location.x > maxPos.x)
//        {
//            location = new Vector3(minPos.x, location.y);
//        }
//        else if (location.x < minPos.x)
//        {
//            location = new Vector3(maxPos.x, location.y);
//        }
//        if (location.y > maxPos.y)
//        {
//            location = new Vector3(location.x, minPos.y);
//        }
//        else if (location.y < minPos.y)
//        {
//            location = new Vector3(location.x, maxPos.y);
//        }
//    }

//    private void lookForward()
//    {
//        /* We want our boids to face the same direction
//         * that they're going. To do that, we take our location
//         * and velocity to see where we're heading. */
//        Vector3 futureLocation = location + velocity;
//        myVehicle.transform.LookAt(futureLocation); // We can use the built in 'LookAt' function to automatically face us the right direction

//        /* In the case our model is facing the wrong direction,
//         * we can adjust it using Eular Angles. */
//        Vector3 eular = myVehicle.transform.rotation.eulerAngles;
//        myVehicle.transform.rotation = Quaternion.Euler(eular.x + 90, eular.y + 0, eular.z + 0); // Adjust these numbers to make the boids face different directions!
//    }
//}

//class BoidChild
//{
//    // To make it easier on ourselves, we use Get and Set as quick ways to get the location of the vehicle
//    public Vector3 location
//    {
//        get { return myVehicle.transform.position; }
//        set { myVehicle.transform.position = value; }
//    }
//    public Vector3 velocity
//    {
//        get { return rb.velocity; }
//        set { rb.velocity = value; }
//    }

//    private float maxSpeed, maxForce;
//    private Vector3 minPos, maxPos;
//    public GameObject myVehicle;
//    private Rigidbody rb;

//    public BoidChild(Vector3 initPos, Vector3 _minPos, Vector3 _maxPos, float _maxSpeed, float _maxForce, Mesh coneMesh)
//    {
//        minPos = _minPos - Vector3.one;
//        maxPos = _maxPos + Vector3.one;
//        maxSpeed = _maxSpeed;
//        maxForce = _maxForce;

//        myVehicle = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        Renderer renderer = myVehicle.GetComponent<Renderer>();
//        renderer.material = new Material(Shader.Find("Diffuse"));
//        renderer.material.color = Color.red;
//        GameObject.Destroy(myVehicle.GetComponent<BoxCollider>());

//        myVehicle.transform.position = new Vector3(initPos.x, initPos.y);

//        myVehicle.AddComponent<Rigidbody>();
//        rb = myVehicle.GetComponent<Rigidbody>();
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.useGravity = false; // Remember to ignore gravity!


//        /* We want to double check if a custom mesh is
//         * being used. If not, we will scale a cube up
//         * instead ans use that for our boids. */
//        if (coneMesh != null)
//        {
//            MeshFilter filter = myVehicle.GetComponent<MeshFilter>();
//            filter.mesh = coneMesh;
//        }
//        else
//        {
//            myVehicle.transform.localScale = new Vector3(1f, 2f, 1f);
//        }
//    }

//    private void checkBounds()
//    {
//        if (location.x > maxPos.x)
//        {
//            location = new Vector3(minPos.x, location.y);
//        }
//        else if (location.x < minPos.x)
//        {
//            location = new Vector3(maxPos.x, location.y);
//        }
//        if (location.y > maxPos.y)
//        {
//            location = new Vector3(location.x, minPos.y);
//        }
//        else if (location.y < minPos.y)
//        {
//            location = new Vector3(location.x, maxPos.y);
//        }
//    }

//    private void lookForward()
//    {
//        /* We want our boids to face the same direction
//         * that they're going. To do that, we take our location
//         * and velocity to see where we're heading. */
//        Vector3 futureLocation = location + velocity;
//        myVehicle.transform.LookAt(futureLocation); // We can use the built in 'LookAt' function to automatically face us the right direction

//        /* In the case our model is facing the wrong direction,
//         * we can adjust it using Eular Angles. */
//        Vector3 eular = myVehicle.transform.rotation.eulerAngles;
//        myVehicle.transform.rotation = Quaternion.Euler(eular.x + 90, eular.y + 0, eular.z + 0); // Adjust these numbers to make the boids face different directions!
//    }

//    public void Flock(List<BoidChild> boids, float sepM, float aliM, float cohM)
//    {
//        Vector3 sep = Separate(boids); // The three flocking rules
//        Vector3 ali = Align(boids);
//        Vector3 coh = Cohesion(boids);

//        sep *= sepM; // Arbitrary weights for these forces (Try different ones!)
//        ali *= aliM;
//        coh *= cohM;

//        ApplyForce(sep); // Applying all the forces
//        ApplyForce(ali);
//        ApplyForce(coh);

//        checkBounds(); // To loop the world to the other side of the screen.
//        lookForward(); // Make the boids face forward.
//    }

//    public Vector3 Align(List<BoidChild> boids)
//    {
//        float neighborDist = 30f; // This is an arbitrary value and could vary from boid to boid.

//        /* Add up all the velocities and divide by the total to
//         * calculate the average velocity. */
//        Vector3 sum = Vector3.zero;
//        int count = 0;
//        foreach (BoidChild other in boids)
//        {
//            if (other != this)
//            {
//                float d = Vector3.Distance(location, other.location);
//                if ((d > 0) && (d < neighborDist))
//                {
//                    sum += other.velocity;
//                    count++; // For an average, we need to keep track of how many boids are within the distance.
//                }
//            }
//        }

//        if (count > 0)
//        {
//            sum /= count;

//            sum = sum.normalized * maxSpeed; // We desite to go in that direction at maximum speed.

//            Vector3 steer = sum - velocity; // Reynolds's steering force formula.
//            steer = Vector3.ClampMagnitude(steer, maxForce);
//            return steer;
//        }
//        else
//        {
//            return Vector3.zero; // If we don't find any close boids, the steering force is Zero.
//        }
//    }

//    public Vector3 Cohesion(List<BoidChild> boids)
//    {
//        float neighborDist = 30f;
//        Vector3 sum = Vector3.zero;
//        int count = 0;
//        foreach (BoidChild other in boids)
//        {
//            if (other != this)
//            {
//                float d = Vector3.Distance(location, other.location);
//                if ((d > 0) && (d < neighborDist))
//                {
//                    sum += other.location; // Adding up all the other's locations
//                    count++;
//                }
//            }
//        }
//        if (count > 0)
//        {
//            sum /= count;
//            /* Here we make use of the Seek() function we wrote in
//             * Example 6.8. The target we seek is thr average
//             * location of our neighbors. */
//            return Seek(sum);
//        }
//        else
//        {
//            return Vector3.zero;
//        }
//    }

//    public Vector3 Seek(Vector3 target)
//    {
//        Vector3 desired = target - location;
//        desired.Normalize();
//        desired *= maxSpeed;
//        Vector3 steer = desired - velocity;
//        steer = Vector3.ClampMagnitude(steer, maxForce);

//        return steer;
//    }

//    public Vector3 Separate(List<BoidChild> boids)
//    {
//        Vector3 sum = Vector3.zero;
//        int count = 0;

//        float desiredSeperation = myVehicle.transform.localScale.x * 2;

//        foreach (BoidChild other in boids)
//        {
//            if (other != this)
//            {
//                float d = Vector3.Distance(other.location, location);

//                if ((d > 0) && (d < desiredSeperation))
//                {
//                    Vector3 diff = location - other.location;
//                    diff.Normalize();

//                    diff /= d;

//                    sum += diff;
//                    count++;
//                }
//            }
//        }

//        if (count > 0)
//        {
//            sum /= count;

//            sum *= maxSpeed;

//            Vector3 steer = sum - velocity;
//            steer = Vector3.ClampMagnitude(steer, maxForce);


//            return steer;
//        }
//        return Vector3.zero;
//    }

//    public void ApplyForce(Vector3 force)
//    {
//        rb.AddForce(force);
//    }
//}