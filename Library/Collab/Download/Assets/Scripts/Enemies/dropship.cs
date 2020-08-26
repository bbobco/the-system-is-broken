using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropship : MonoBehaviour
{
    bool opening = false;
    float timeOpened = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<joyridingpoop>().manualControl = true;
        GetComponent<joyridingpoop>().target = GameObject.Find("player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject player = GameObject.Find("player");
        
        if ((transform.position - player.transform.position).magnitude < 60 && !opening)
        {
            timeOpened = Time.time;

            GetComponent<joyridingpoop>().target = GameObject.Find("grass planet");
            opening = true;
            openDoor();
        }
        if(opening)
            openDoor();

    }

    float s = 8;
    bool opened = false;
    void openDoor()
    {
        opened = true;

        Transform door = transform.GetChild(0);
        Quaternion q = new Quaternion();
        q.SetFromToRotation(Vector3.right,-Vector3.up);
        q = Quaternion.Slerp(door.rotation, q, Time.fixedDeltaTime * s);

        if (Time.time - timeOpened > 0.35f &&!opened)
        {
            for (int x = 2; x < 6; x++)
            {
                transform.GetChild(x).gameObject.GetComponent<badguy>().chasingPlayer = true;
                transform.GetChild(x).parent = null;
            }
            timeOpened = 0;
        }


        //s += Time.fixedDeltaTime;
        door.rotation = q;

    }
}
