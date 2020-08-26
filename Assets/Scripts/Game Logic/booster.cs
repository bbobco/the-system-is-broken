using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class booster : MonoBehaviour
{

    public GameObject targetPlanet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("player"))
        {
            Vector3 direction = (targetPlanet.transform.position - transform.position).normalized;
            Vector3 normal = (collision.gameObject.transform.position - collision.gameObject.GetComponent<GravityBody>().winner.transform.position).normalized;
            Vector3 boostdirection = (direction / 2 + normal / 5).normalized;

            int boostforce = 7000;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(boostdirection * boostforce);
        }

    }
}
