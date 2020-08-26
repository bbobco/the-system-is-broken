using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeEnemies : MonoBehaviour
{
    public int radius = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] boomers = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider b in boomers)
        {
            if (b.gameObject.tag == "enemy")
            { 
            
                b.gameObject.GetComponent<shrinkonshot>().sizeofdeath++;
                b.gameObject.GetComponent<shrinkonshot>().death();
                b.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}