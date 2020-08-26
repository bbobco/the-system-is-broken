using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosiveShot : MonoBehaviour
{
    public GameObject explosion;
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
        Destroy(this.gameObject, 1);
        var d =Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(d, 2.5f);
    }
}
