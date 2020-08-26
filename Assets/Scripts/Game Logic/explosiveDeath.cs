using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class explosiveDeath : MonoBehaviour
{
    public int hp = 10;
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
        //UnityEngine.Debug.Log("fdjsdkgjgdsjkkj");
       // if (other.gameObject == this.gameObject) return;

        if (other.tag == "playerAttack" || other.tag == "enemybullet")
        {
            hp--;

            UnityEngine.Debug.Log("u hit me dawg");
            if (hp > 0) return;
            
            foreach (Collider c in Physics.OverlapSphere(transform.position, 300))
            {
                int explosiveForce = 1500;
                Vector3 dir =(c.transform.position-transform.position).normalized;
                Rigidbody r = c.gameObject.GetComponent<Rigidbody>();
                if(r!=null)
                    r.AddForce(dir*explosiveForce);
            }
            Destroy(other.gameObject, 1);
            foreach (Transform t in transform)
            {
                var rb = t.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                if (rb != null)
                    rb.AddExplosionForce(UnityEngine.Random.Range(70, 430)*0+65, transform.position, 30);
            }
            Destroy(GetComponent<GravityAttractor>(), 1);
           // Destroy(this, 10);
        }

    }
}
