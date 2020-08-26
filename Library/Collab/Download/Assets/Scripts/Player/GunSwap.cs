using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GunSwap : MonoBehaviour
{
    private GameObject[] guns;
    private int numWeapons;
    public int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        guns = Resources.LoadAll("Guns", typeof(GameObject)).Cast<GameObject>().ToArray();
        numWeapons = guns.Length;
        Debug.Log(numWeapons);
    }

    // Update is called once per frame
    void Update()
    {
        int prevIndex = currentIndex;
        if (Input.GetKeyDown("1"))
            currentIndex = 0;
        else if (Input.GetKeyDown("2"))
            currentIndex = 1;
        else if (Input.GetKeyDown("3"))
            currentIndex = 2;
        else if (Input.GetKeyDown("4"))
            currentIndex = 3;
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && currentIndex < numWeapons - 1)
            currentIndex++;
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && currentIndex >= numWeapons - 1)
            currentIndex = 0;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndex > 0)
            currentIndex--;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndex == 0)
            currentIndex = numWeapons - 1;

        // change weapon
        if ( currentIndex != prevIndex)
        {
           foreach ( Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            GameObject newGun = Instantiate(guns[currentIndex], transform);
        }

    }
}
