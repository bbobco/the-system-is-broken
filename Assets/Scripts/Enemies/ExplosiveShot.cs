using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShot : MonoBehaviour
{
    public GameObject explosion;
    public float size = 1;
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
        if(other.gameObject.name=="player") return;
        Destroy(this.gameObject, 0);
        var p =Instantiate(explosion, transform.position, Quaternion.identity);
        p.transform.localScale *= size;
        Destroy(p.transform.root.gameObject, 2.5f);
    }
}
