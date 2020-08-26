using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletHitEffect : MonoBehaviour
{
    public GameObject effect;
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
        if (collision.gameObject.name.Equals("player")) return;
        var p = Instantiate(effect, transform.position, Quaternion.identity);
    //    p.transform.localScale *= .04f;
        Destroy(transform.root.gameObject, 0);
        Destroy(p.transform.root.gameObject, 2.5f);
    }
}