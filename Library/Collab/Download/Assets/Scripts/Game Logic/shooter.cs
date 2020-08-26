using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class shooter : MonoBehaviour
{
    float lastshot = 0;
    int numfired = 0;
    // Start is called before the first frame update
    public GameObject projectile;
    List<GameObject> minions = new List<GameObject>();
    int numMinions = 15;
    //GameObject[] minions = new GameObject[15];
    void Start()
    {
        
        
        for (int x = 0; x < numMinions; x++)
        {
            //    minions = new GameObject[numMinions];

            // temporary fix
            //float magnitude = transform.localScale.magnitude;
            float magnitude = 17.5f;

            Vector3 dir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            var p = Instantiate(projectile, (transform.position + dir * (magnitude + 4.5f)), Quaternion.identity);     
            p.GetComponent<Rigidbody>().velocity += GetComponent<Rigidbody>().velocity;
            minions.Add(p.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time - lastshot > 1.25f &&numfired<numMinions) 
        {
          //  if(minions[numfired==null])
            lastshot = Time.time;
            Vector3 dir = (GameObject.Find("player").GetComponent<GravityBody>().winner.transform.position - minions[numfired].transform.position).normalized;
            int projectilespeed = 2500;
            minions[numfired].GetComponent<Rigidbody>().velocity = Vector3.zero;
            minions[numfired].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            minions[numfired].GetComponent<Rigidbody>().AddForce(dir*projectilespeed);
            minions[numfired].GetComponent<badguy>().chasingPlayer = true;
            minions[numfired].GetComponent<GravityBody>().immune = true;
            numfired++;

        }
        
    }
}
