using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    GameObject daddy;
    Rigidbody MyRigidBody;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        MyRigidBody = GetComponent<Rigidbody>();
        float closest = 0;
        foreach (GameObject p in planets)
        {
            if (p.gameObject.name.Equals(gameObject.name)) continue;
            float dist = (MyRigidBody.transform.position - p.transform.position).magnitude;
            if (dist < closest || closest == 0)
            {
                closest = dist;
                daddy = p;
            }
        }
        transform.LookAt(daddy.transform.position);
        closest /= 10;
        closest = Mathf.Pow(closest,2-GravityAttractor.distPow);
        float gmass = daddy.GetComponent<GravityAttractor>().gmass;
        //took the real orbital speed equation and had to ghetto modify it to actually work
        float orbitalspeed = Mathf.Sqrt(15.0f*GravityAttractor.gravity * gmass*(3.4f) / closest  * 1);//12;//Mathf.Sqrt(700/closest);

        int random = UnityEngine.Random.Range(1,4);

        if(random==1)
            MyRigidBody.velocity = transform.up * orbitalspeed;
        if (random == 2)
            MyRigidBody.velocity = -transform.up * orbitalspeed;
        if (random == 3)
            MyRigidBody.velocity = transform.right * orbitalspeed;
        if (random == 4)
            MyRigidBody.velocity = -transform.right * orbitalspeed;


    }

        // Update is called once per frame
     void Update()
    {
       // UnityEngine.Debug.Log("hurpa  " + transform.right);
    }
}
