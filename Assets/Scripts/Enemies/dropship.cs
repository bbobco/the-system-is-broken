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


            opening = true;
            openDoor();
        }
        if(opening)
            openDoor();

    }

    float s = 0;
    bool opened = false;
    void openDoor()
    {

        if (s < 1)
        {
            s += Time.fixedDeltaTime/7f;
            Transform door = transform.GetChild(0);
            Quaternion q = new Quaternion();
            q.SetFromToRotation(Vector3.right, -Vector3.up);
            q = Quaternion.Slerp(door.rotation, q, s);
            door.rotation = q;
        }
        if (Time.time - timeOpened > 3.75f && !opened)
        {
            GetComponent<joyridingpoop>().target = GameObject.Find("grass planet");
            foreach(Transform child in transform)
            {
                if (!child.gameObject.tag.Equals("enemy")) continue;
                child.gameObject.GetComponent<badguy>().chasingPlayer = true;
                child.parent = null;
            }
            timeOpened = 0;
            opened = true;
        }

        
        //s += Time.fixedDeltaTime;
        

    }
}
