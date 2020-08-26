using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drillFX : MonoBehaviour
{
    public GameObject dirt;
    public GameObject rubble;
    GameObject livedirt = null;
    GameObject liverubble=null;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag != "asteroid") return;
        transform.parent.gameObject.GetComponent<miner>().mining = false;

    }
    void OnCollisionStay(Collision other)
    {
        
        if (other.gameObject.tag != "asteroid") return;
        transform.parent.gameObject.GetComponent<miner>().mining = true;
        if (Time.time-timer>.15f)
        {
            UnityEngine.Debug.Log("drill tip");
            timer = Time.time;
            var p = Instantiate(dirt, other.contacts[0].point, Quaternion.identity);
            Destroy(p, 0.75f);
            p.transform.parent = transform;
            //Instantiate(rubble, other.contacts[0].point, Quaternion.identity);
        }
    }
}
