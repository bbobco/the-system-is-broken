using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : MonoBehaviour
{
    [Header("Timer")]
    //Time before the grenade explodes
    [Tooltip("Time between spawns")]
    public float spawnTime = 5.0f;
    //Y degree rotation
    [Tooltip("Original angle in degrees")]
    public int angleDegrees = 0;
    //Y degree rotation
    [Tooltip("Rotation between spawns")]
    public int angleIterator = 0;
    [Header("Object")]
    //Spawning object
    [Tooltip("The thing that will spawn")]
    public GameObject prefab;
    [Tooltip("The speed of the projectile")]
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        //Start the explosion timer
        StartCoroutine(spawnTimer());
    }
    private IEnumerator spawnTimer ()
    {
        yield return new WaitForSeconds(spawnTime);
        angleDegrees += angleIterator;
        if (angleDegrees > 359) angleDegrees = 0;
        Quaternion rot = Quaternion.Euler(-30, angleDegrees, 0);
        GameObject spawn = Instantiate(prefab, transform.position, rot);
        GameObject player = GameObject.Find("player");
        spawn.AddComponent<GravityBody>();
        Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);
        spawn.transform.LookAt(target);
        Rigidbody spawnbody = spawn.GetComponent<Rigidbody>();
        spawnbody.useGravity = false;
        spawnbody.velocity = spawn.transform.forward * speed;
        StartCoroutine(spawnTimer() );
        Destroy(spawn, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
