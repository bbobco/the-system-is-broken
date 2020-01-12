using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRotator : MonoBehaviour
{
    public float timeIncrement = 0F;
    public float myTime = 0F;
    Vector3 perlinSeeds;
    Vector3 vRot;
    // Start is called before the first frame update
    void Start()
    {
        perlinSeeds = new Vector3(Random.Range(0f, 100f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        myTime += timeIncrement;
        vRot.x = Mathf.PerlinNoise(myTime, perlinSeeds.x);
        vRot.y = Mathf.PerlinNoise(myTime, perlinSeeds.y);
        vRot.z = Mathf.PerlinNoise(myTime, perlinSeeds.z);

        gameObject.transform.Rotate(vRot);
    }
}
