using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapModelsAndExplode : MonoBehaviour
{
    public GameObject fracsteroid;
    bool exploded = false;
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
        //UnityEngine.Debug.Log("ASTEROID HIT");
        if (exploded) return;
        
        if (other.tag == "playerAttack" || other.tag == "enemybullet")
        {
            exploded = true;
       //     UnityEngine.Debug.Log("ASTEROID HIT");
            var created_asteroid = Instantiate(fracsteroid, transform.position, Quaternion.identity);
            created_asteroid.transform.position = transform.position;
            created_asteroid.transform.localScale = transform.localScale/=100f;
            created_asteroid.transform.rotation = transform.rotation;
            /*
            foreach (Collider c in Physics.OverlapSphere(transform.position, 300))
            {
                int explosiveForce = 1500;
                Vector3 dir = (c.transform.position - transform.position).normalized;
                Rigidbody r = c.gameObject.GetComponent<Rigidbody>();
                if (r != null)
                    r.AddForce(dir * explosiveForce);
            }
            */
           // Destroy(other.gameObject, 1);
            foreach (Transform t in created_asteroid.transform)
            {
                var rb = t.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                if (rb != null)
                    rb.AddExplosionForce(UnityEngine.Random.Range(70, 430)  + 100, transform.position, 30);

            }
            //this.gameObject.tag = "Untagged";
            Destroy(transform.root.gameObject, 1);
            Destroy(created_asteroid, 7);

          //  Destroy(other.gameObject, 1);
        }
    }
}
