using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class badguy : MonoBehaviour
{
    public GameObject[] planets;
    GameObject player;
    GameObject daddy = null;
    GameObject newdaddy = null;

    float lastchange = 0;
    float lastjumped = 0;
    bool gotdir = false;
    bool jumping = false;
    bool jumpcompleted= false;
    Vector3 upcomingdir;
    Vector3 lastdir;
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
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (Time.time-7f>lastjumped)
        {
            jumpcompleted = false;
            jumping = jump();
            lastjumped = Time.time+1000f;
        }
        if (jumping)
        {
            lastjumped = 21000f;
            upcomingdir = (newdaddy.transform.position- transform.position).normalized;
            float distFromGround = (transform.position - newdaddy.transform.position).magnitude - newdaddy.transform.localScale.x / 2;
            if (distFromGround < 5)
            {
                lastjumped = Time.time;
                jumping = !jumping;
            }

        }
        if (Time.time - lastchange > 0.25f&&!jumping)
        {

            float closest = 0;
            GameObject lastdaddy = daddy;
            foreach (GameObject p in planets)
            {
                if (p.gameObject.name.Equals(gameObject.name)) continue;
                float dist = (transform.position - p.transform.position).magnitude;
                if (dist < closest || closest == 0)
                {
                    closest = dist;
                    daddy = p;
                }
            }
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
    }

    bool jump()
    {
        if (jumpcompleted) return true;
        foreach (GameObject p in planets)
        {
            if (p != daddy)
                newdaddy = p;
        }
        
            
            
            Vector3 normal = (transform.position - daddy.transform.position).normalized;

            UnityEngine.Debug.Log("WEATKTTTTTTTTT");
            GetComponent<Rigidbody>().AddForce(normal * 1800);

            jumpcompleted = true;

        return true;
    }
}
