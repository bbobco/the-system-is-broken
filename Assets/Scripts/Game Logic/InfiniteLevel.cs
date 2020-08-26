using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteLevel : MonoBehaviour
{   
    public float LevelBounds = 100;
    private GameObject planet;
    private int CloneScenes = 2;

    // Start is called before the first frame update
    void Start()
    {
        planet = GameObject.FindGameObjectsWithTag("Planet")[0];
        for ( int i = -CloneScenes; i < CloneScenes; i++ )
            for ( int j = -CloneScenes; j < CloneScenes; j++)
                for (int k = -CloneScenes; k < CloneScenes; k++) {
                    Vector3 offset = new Vector3(i*2*LevelBounds, j*2*LevelBounds, k*2*LevelBounds);
                    GameObject planetClone = GameObject.Instantiate(planet, planet.transform.position + offset, planet.transform.rotation);
                    Destroy(planetClone.GetComponent<MeshCollider>());
                    Destroy(planetClone.GetComponent<GravityAttractor>());
                }
                    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;
        if ( p.x > LevelBounds ) 
            p.Set( p.x - 2*LevelBounds, p.y, p.z);
        if ( p.y > LevelBounds ) 
            p.Set( p.x, p.y - 2*LevelBounds, p.z);
        if ( p.z > LevelBounds ) 
            p.Set( p.x, p.y, p.z - 2*LevelBounds);

        if ( p.x < -LevelBounds ) 
            p.Set( 2*LevelBounds + p.x, p.y, p.z);
        if ( p.y < -LevelBounds ) 
            p.Set( p.x,  2*LevelBounds + p.y, p.z);
        if ( p.z < -LevelBounds ) 
            p.Set( p.x, p.y, 2* LevelBounds + p.z);  

        transform.position = p;      
    }
}
