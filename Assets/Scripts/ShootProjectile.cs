using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody MyProjectile;
    public Transform cameraTransform;
    public float speed = 10F;
    public bool fire = false;


    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fire = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            fire = false;
        }


    }

    void FixedUpdate()
    {
        if (fire)
        {
            Transform projectileSpawn = transform;
            // projectileSpawn.transform.position += transform.forward * speed;
            Rigidbody projectile = Instantiate(MyProjectile, projectileSpawn.position, projectileSpawn.rotation);
            projectile.velocity = cameraTransform.forward * speed;
            projectile.transform.position += cameraTransform.forward * 2;
        }
    }
}
