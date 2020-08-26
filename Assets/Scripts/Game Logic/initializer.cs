using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializer : MonoBehaviour
{
    public GameObject asteroid1;
    public GameObject asteroid2;
    public bool durka;
    public GameObject squadron;
    public GameObject shooter;
    public GameObject shooterNinja;
    float diameter;
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        int numasteroids = 480/2/2;
        float maxsize = 1.9f*6*4/2; 
        for (int x = 0; x < numasteroids; x++)
        {
            int randorange = 420;
            GameObject[] asteroids = { asteroid1, asteroid2 };
            var created_asteroid = Instantiate(asteroid2, new Vector3(UnityEngine.Random.Range(-randorange, randorange), UnityEngine.Random.Range(-randorange, randorange - 20), UnityEngine.Random.Range(-randorange, randorange)), Quaternion.identity);
            diameter = UnityEngine.Random.Range(3.2f*3, maxsize);
            //diameter = UnityEngine.Random.Range(1, 10f);
            Vector3 dir = new Vector3(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50));
            dir.Normalize();
            created_asteroid.transform.localScale = new Vector3(diameter, diameter, diameter);

            int spinspeed = UnityEngine.Random.Range(0,10);
            Vector3 randomspin = new Vector3(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50)).normalized;
            created_asteroid.GetComponent<Rigidbody>().AddTorque(randomspin*spinspeed,ForceMode.VelocityChange);

            created_asteroid.tag = "asteroid";

        //    UnityEngine.Debug.Log(created_asteroid.tag);
        }
        for (int x = 0; x < 0; x++)
        {

            var created_squad= Instantiate(squadron, new Vector3(UnityEngine.Random.Range(-randorange, randorange), UnityEngine.Random.Range(-randorange, randorange - 20), UnityEngine.Random.Range(-randorange, randorange)), Quaternion.identity);
       }


    }
    int randorange = 380;
    float lastSquad = 0;
    float lastShooter;
    int numshooters;
    int numsquads = 0;
    void FixedUpdate()
    { 
        if (! durka ) {
            if (Time.time - lastSquad > 7f && numsquads<4)
            {
                var created_squad = Instantiate(squadron, new Vector3(UnityEngine.Random.Range(-randorange, randorange), UnityEngine.Random.Range(-randorange, randorange - 20), UnityEngine.Random.Range(-randorange, randorange)), Quaternion.identity);
                lastSquad = Time.time;
                numsquads++;
            }
            
            if (Time.time - lastShooter > 15f && numshooters<10)
            {
                int choice = UnityEngine.Random.Range(0, 1);
                numshooters++;
                if(choice==0)
                    Instantiate(shooter, new Vector3(UnityEngine.Random.Range(-randorange, randorange), UnityEngine.Random.Range(-randorange, randorange - 20), UnityEngine.Random.Range(-randorange, randorange)), Quaternion.identity);
                if (choice==1)
                    Instantiate(shooterNinja, new Vector3(UnityEngine.Random.Range(-randorange, randorange), UnityEngine.Random.Range(-randorange, randorange - 20), UnityEngine.Random.Range(-randorange, randorange)), Quaternion.identity);

                lastShooter = Time.time;
            }
        }
            
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
