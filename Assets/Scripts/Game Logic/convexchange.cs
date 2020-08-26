using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class convexchange : MonoBehaviour
{
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
        if (other.name == "player")
        {
            UnityEngine.Debug.Log("WATWATWAT");
            GameObject.Find("WAT").GetComponent<MeshCollider>().convex = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "player")
        {
            UnityEngine.Debug.Log("TAWTAWTAWTAW");
            GameObject.Find("WAT").GetComponent<MeshCollider>().convex = true;
        }
    }

}
