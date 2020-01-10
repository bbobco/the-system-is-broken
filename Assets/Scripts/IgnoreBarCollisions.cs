using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreBarCollisions : MonoBehaviour
{

    SphereCollider myCollider;

    void Start()
    {
        myCollider = gameObject.GetComponent(typeof(SphereCollider)) as SphereCollider;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BarChart")
        {
            Physics.IgnoreCollision(collision.collider, myCollider);
        }
    }
}
