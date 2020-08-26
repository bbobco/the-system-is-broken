using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject shooter;
    public GameObject shooterNinja;
    public GameObject asteroid;
    public GameObject planet1;
    public GameObject planet2;
    public GameObject fighter;
    public GameObject gazebo;

    GameObject[] planets;
    float AreaSize = 400;
    void     Start()
    {
        int numplanets = UnityEngine.Random.Range(1, 3);
        int numfighters = UnityEngine.Random.Range(2,4);
        int minAsteroids = 100;
        int maxAsteroids = 200;
        spawnPlanet(planet1);
        spawnPlanet(planet2);
        spawnAsteroids(minAsteroids,maxAsteroids);
        for(int x=0;x<numfighters;x++)
            spawnFighter();
    }
    void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnPlanet(GameObject p)
    {
        int spawnRange = 400;
        int minSize = 55;
        int maxSize = 180;
        var sp = Instantiate(p,joyridingpoop.randomvec()*spawnRange,Quaternion.identity);
        float size = UnityEngine.Random.Range(minSize, maxSize);
        sp.transform.localScale = new Vector3(size,size,size);
        int shooters = UnityEngine.Random.Range(1, 3);
        for(int x=0;x<shooters;x++)
        {
            spawnShooter(sp.transform.position,size);
        }
        


    }

    void spawnShooter(Vector3 host,float size)
    {
        int shootertype = UnityEngine.Random.Range(0, 1);
        int numMinions = UnityEngine.Random.Range(4,20);
        float scale = UnityEngine.Random.Range(36,78);
        float distFromPlanet = size / 2 + UnityEngine.Random.Range(1.55f*size, 2.9f*size);
        var s = 1;
        if (shootertype == 0)
            Instantiate(shooter,host+joyridingpoop.randomvec()*distFromPlanet,Quaternion.identity).transform.localScale = new Vector3(scale, scale, scale);
        if (shootertype == 1)
            Instantiate(shooterNinja, host + joyridingpoop.randomvec() * distFromPlanet, Quaternion.identity).transform.localScale = new Vector3(scale, scale, scale); ;

    }
   

    void spawnAsteroids(int minasteroids,int maxasteroids)
    {
        float minAsteroidSize = 25;
        float maxAsteroidSize = 80;
        int numAsteroids = UnityEngine.Random.Range(minasteroids,maxasteroids);
        for(int x=0;x<numAsteroids;x++)
        {
            var a = Instantiate(asteroid,joyridingpoop.randomvec()*AreaSize,Quaternion.identity);
            float asize = UnityEngine.Random.Range(minAsteroidSize, maxAsteroidSize);
        }

    }

    void spawnFighter()
    {
        var f= Instantiate(fighter, joyridingpoop.randomvec() * AreaSize, Quaternion.identity);
    }
}
