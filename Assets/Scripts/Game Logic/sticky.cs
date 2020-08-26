using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sticky : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
    private void OnTriggerStay(Collider other)
    {
        float speedcap = 9;
        if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude>speedcap)
            other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity.normalized*speedcap;

        if (other.gameObject.tag.Equals("enemy"))
            if (other.gameObject.GetComponent<badguy>() != null)
                other.gameObject.GetComponent<badguy>().canjump = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("enemy"))
            if (other.gameObject.GetComponent<badguy>() != null)
                other.gameObject.GetComponent<badguy>().canjump = true;
                
    }
    */
}
