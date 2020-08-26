using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class miner : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ast;
    float distToMine = 20;
    float miningtime = 0;
    bool looking = true;
    bool goinghome = false;
    public bool mining = false;
    Quaternion spin = new Quaternion();
    void Start()
    {
        spin = Quaternion.FromToRotation(Vector3.forward,new Vector3(0,-.13927f,.9903f));
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if(looking)
        {
            while (looking)
            {
                ast = GameObject.FindGameObjectsWithTag("asteroid")[UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("asteroid").Length - 1)];
                if ((GameObject.Find("player").transform.position - ast.transform.position).magnitude > 200)
                    continue;
                if (ast.transform.localScale.x < 2)
                    continue;
                //ast.GetComponent<GravityBody>().winner = GameObject.Find("player").GetComponent<GravityBody>().winner;
                //UnityEngine.Debug.Log("ast "+ ast.transform.position+ast.transform.forward*ast.transform.localScale.z);
                looking = false;
            }
            GetComponent<joyridingpoop>().target = ast;
            distToMine = transform.localScale.z / 2 + ast.transform.localScale.x / 2 + 6;
            

        }
        if (!goinghome)
        {
            if (mining)
            {
                miningtime += Time.fixedDeltaTime;
                if (miningtime > 10f) { goinghome = true; miningtime = 0; GetComponent<joyridingpoop>().target = GameObject.Find("GAZEBOobj"); GetComponent<joyridingpoop>().speedmultiplier = 5; GetComponent<joyridingpoop>().overheadDistance = 60; mining = false; }

                ast.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ast.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                transform.LookAt(ast.transform.position + ast.transform.forward * ast.transform.localScale.z);

                GetComponent<joyridingpoop>().speedmultiplier = 1.5f;
                /*
                float s = 8;
                Transform door = transform.GetChild(1);
                Quaternion q = new Quaternion();
                q.SetFromToRotation(Vector3.right, -Vector3.up);
                q = Quaternion.Slerp(door.rotation, q, Time.fixedDeltaTime * s);
                */
                //  transform.GetChild(0).GetChild(0).rotation = spin*transform.GetChild(0).GetChild(0).rotation;

                // transform.GetChild(0).GetChild(0).RotateAround(new Vector3(5, 0, 0), Space.Self);

                Transform child = transform.GetChild(0).GetChild(0);
                child.RotateAround(child.position + child.transform.up * .9f, child.right, 900f * Time.fixedDeltaTime);

            }
        }
    }
}
