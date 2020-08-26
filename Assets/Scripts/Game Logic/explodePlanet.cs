using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class explodePlanet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        explode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void explode()
    {
        foreach(Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(UnityEngine.Random.Range(70,430),transform.position,30);
        }
    }
}
