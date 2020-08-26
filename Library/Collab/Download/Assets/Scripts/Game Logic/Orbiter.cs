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
        float orbitalspeed = Mathf.Sqrt(10.0f*700 * daddy.GetComponent<GravityAttractor>().gmass / closest  * 1);//12;//Mathf.Sqrt(700/closest);

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
