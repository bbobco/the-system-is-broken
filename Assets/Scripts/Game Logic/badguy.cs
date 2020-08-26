using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
//using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using UnityEngine;

public class badguy : MonoBehaviour
{
    public GameObject[] planets;
    GameObject player;
    GameObject daddy = null;
    GameObject newdaddy = null;

    public bool useTheForce = true;
    public float speed = 1;
    public bool chasingPlayer = false;
    Vector3 arbloc; //used for each time to path around a sphere to make sure we continue on same path
    float lastchange = 0;
    float lastjumped = 0;
    bool gotdir = false;
    bool jumping = false;
    bool jumpcompleted= false;
    Vector3 upcomingdir;
    Vector3 lastdir;
    int framecounter=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");

        transform.localEulerAngles = Vector3.left;
        /*
        planets = new GameObject[planets.Length];
        for (int i = 0; i < planets.Length; i++)
        {
           planets[i] = planets[i].GetComponent<GravityAttractor>();
        }
        */
        arbloc = new Vector3(UnityEngine.Random.Range(-350,350), UnityEngine.Random.Range(-350, 350), UnityEngine.Random.Range(-350, 350));
    }
  
    void FixedUpdate()
    {

        framecounter++;
        if (!jumping)
        {
            
            float closest = 0;
            GameObject lastdaddy = daddy;
            foreach (GameObject p in planets)
            {
                if (p == null) continue;
                if (p.gameObject.name.Equals(gameObject.name)) continue;
                float dist = (transform.position - p.transform.position).magnitude;
                if (dist < closest || closest == 0)
                {
                    closest = dist;
                    daddy = p;
                }
            }
        }
     //   UnityEngine.Debug.Log(daddy.transform.localScale.x);
        GameObject player = GameObject.Find("player");
        if (player.GetComponent<GravityBody>().winner == null) return;
        GravityAttractor playerdaddy = player.GetComponent<GravityBody>().winner;
   //     UnityEngine.Debug.Log(player.GetComponent<GravityBody>().winner.transform.localScale.x);
        int pushforce = 2800;
        if (framecounter % 15 == 0 && chasingPlayer)
        {
            arbloc = joyridingpoop.randomvec()*UnityEngine.Random.Range(-350,350);
            //UnityEngine.Debug.Log((transform.position - playerdaddy.transform.position).magnitude - playerdaddy.transform.localScale.x / 2);
            
            if ((transform.position - playerdaddy.transform.position).magnitude- playerdaddy.transform.localScale.x >  15 &&useTheForce)
            {
           //    UnityEngine.Debug.Log("dsgdsjdsgjsdgskjdkjkj");
                upcomingdir = (playerdaddy.transform.position - transform.position).normalized;
                pushforce*=1;
                //GetComponent<Rigidbody>().AddForce(upcomingdir * pushforce/100,ForceMode.VelocityChange);
            }
            
           else 
            {
                upcomingdir = pathingOnSphere(transform.position - daddy.transform.position, transform.position, player.transform.position,arbloc);
                if (useTheForce)
                    GetComponent<Rigidbody>().AddForce(upcomingdir * pushforce * Time.fixedDeltaTime);

            }
            framecounter = 0;

        }
      //  UnityEngine.Debug.Log("trying to move " + upcomingdir);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (useTheForce)
            rb.AddForce(upcomingdir * pushforce * Time.fixedDeltaTime);
        else
        {
            UnityEngine.Debug.Log("trying to move " + upcomingdir);
            transform.position += transform.forward * Time.fixedDeltaTime *4;
        }
        if (rb.velocity.magnitude > 100)
            rb.velocity *= 100 / rb.velocity.magnitude;

        float distFromGround = (transform.position - playerdaddy.transform.position).magnitude - playerdaddy.transform.localScale.x / 2;
        bool grounded = distFromGround < 15;
        if (Time.time-4f>lastjumped && grounded)
        {
         //   jumpcompleted = false;
            jumping = jump();
            lastjumped = Time.time;
        }
        if (jumping)
        {
    //        upcomingdir = (daddy.transform.position- transform.position).normalized;
           
            if (grounded)
            {
                jumping = false;
            }

        }

        //*********************** some sick/SUPER IMPORTANT MATH right here
        Quaternion q = new Quaternion();
        Vector3 directlook = (player.transform.position - transform.position).normalized;
        Vector3 normal = (transform.position - playerdaddy.transform.position).normalized;
        Vector3 intermediate = Vector3.Cross(normal, directlook);
        Vector3 levelToGroundLook = Vector3.Cross(intermediate, normal);
        //**********************


            q.SetFromToRotation(transform.forward, levelToGroundLook);
            q = q * transform.rotation;
           transform.rotation = Quaternion.Slerp(transform.rotation,q,.09f);
        
        //transform.LookAt(player.transform.position);

        /*
    //    if (daddy != lastdaddy) YOLO = false;
        transform.LookAt(daddy.transform.position);
        Vector3 normal = (transform.position - daddy.transform.position).normalized;
        Vector3 rando = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;

        //UnityEngine.Debug.Log(normal);
        // UnityEngine.Debug.Log(rando);

        upcomingdir = Vector3.Cross(normal, rando);
        //upcomingdir = newdir;

        //UnityEngine.Debug.Log(upcomingdir);

        lastchange = Time.time;     
    }
    //transform.localEulerAngles = Vector3.left;
    // UnityEngine.Debug.Log(transform.forward);
    //GetComponent<Rigidbody>().AddForce(transform.forward* 1600 * Time.fixedDeltaTime);
    UnityEngine.Debug.Log(jumping);
    int pushforce = jumping ? 700 :1700;
    GetComponent<Rigidbody>().AddForce(upcomingdir * pushforce * Time.fixedDeltaTime);
    */

    }
    //accurate way of finding a path to another point on a sphere, making sure ONLY to provide a solution along the normal plane of players location
    //there are infinite solutions as to paths to get to the target, so im using arbitrarylocation to provide one solution for each route taken
    //in order to get a solution along the sphere we need any line perpendicular to a plane which contains the line from the location to the target, and the normal of the surface where location is
    public static Vector3  pathingOnSphere(Vector3 normal, Vector3 location, Vector3 target, Vector3 arbitrarylocation)
    {


        //      UnityEngine.Debug.Log("p1 "+(target-location).normalized);
        //      UnityEngine.Debug.Log("p2 " + (arbitrarylocation - target).normalized);
      //  UnityEngine.Debug.Log("ARBBBBB "+arbitrarylocation);
     //   UnityEngine.Debug.Log((target - location).normalized + " " + (target - arbitrarylocation).normalized );
        Vector3 normal2 = Vector3.Cross((target-location).normalized, (target-arbitrarylocation).normalized); //the normal of a plane which contains the line from location to target
        if(normal2.magnitude<0.3f) arbitrarylocation = arbitrarylocation = new Vector3(UnityEngine.Random.Range(-350, 350), UnityEngine.Random.Range(-350, 350), UnityEngine.Random.Range(-350, 350));
        //unfortunately there is always a point where the lines are parallel given our arbitrary point, and it must be modified to not cause a freeze of motion
        /*      if (normal2.magnitude < .1f)
              {
                  arbitrarylocation = new Vector3(UnityEngine.Random.Range(-350, 350), UnityEngine.Random.Range(-350, 350), UnityEngine.Random.Range(-350, 350));
                  normal2 = Vector3.Cross((target - location).normalized, (arbitrarylocation - target).normalized).normalized;

              }*/

        Vector3 normal1 = normal.normalized;// (location - daddy.transform.position).normalized; // the normal of the sphere from point location

      //  UnityEngine.Debug.Log("n1 " + normal1);
     //   UnityEngine.Debug.Log("n2 " + normal2);
        Vector3 solution = Vector3.Cross(normal1, normal2);

    //    UnityEngine.Debug.Log("solution to pathing is " + solution);

 //       UnityEngine.Debug.Log("mag "+solution.magnitude);
  //      UnityEngine.Debug.Log("dot "+Vector3.Dot(solution, (target - location)));
        if (Vector3.Dot(solution,(target-location))<0) 
            solution *= -1;
    //    if(solution.magnitude<0.15f)
       //     return (target - location).normalized;
        return solution.normalized;  //a line on the plane containing a path to target, and our normal plane
    }
    public int jumpforce = 550;
    bool jump()
    {

        // if (jumpcompleted) return true;

        /*
        foreach (GameObject p in planets)
        {
            if (p != daddy)
                newdaddy = p;
        }
        */
            float perPlanetJumpForce = jumpforce * daddy.GetComponent<GravityAttractor>().gmass / 7.5f;
            
            Vector3 normal = (transform.position - daddy.transform.position).normalized;

            UnityEngine.Debug.Log("WEATKTTTTTTTTT");
            float rforce = UnityEngine.Random.Range(perPlanetJumpForce / 3*2, perPlanetJumpForce);
            GetComponent<Rigidbody>().AddForce(normal * rforce);

            jumpcompleted = true;

        return true;
    }
}
