using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class builder : MonoBehaviour
{
    GameObject[] blocks;
    public GameObject block;

    public float checkradius = 1f;
    public bool dense = false;
    public bool tree = false;
    public bool sparse = false;
    public float treeskinny = 1;
    public bool snake = false;
    GameObject last;
    float lastTime = 0;
    int count = 0;
    public GameObject bplanet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(count==0)
        {
            blocks = GameObject.FindGameObjectsWithTag("block");
            if (snake) last = blocks[0];
            Vector3 n = (block.transform.position - bplanet.transform.position).normalized;
            Quaternion qq = new Quaternion();
            qq.SetFromToRotation(Vector3.up, n);
            block.transform.rotation = qq * transform.rotation;
        }

        if (Time.time - lastTime < .15f)
        {
            
            return;
        }
        lastTime = Time.time;

        blocks = GameObject.FindGameObjectsWithTag("block");

        /*
        if (count > 100)
        {
            if (count ==101 && GetComponent<joyridingpoop>().enabled==false)
                GetComponent<joyridingpoop>().enabled = true;
            return;
        }
        */
        //       UnityEngine.Debug.Log(blocks.Length);

        Vector3 newblockLocation = Vector3.zero;
        Vector3 normal = Vector3.zero;
        bool empty = true;
        while (empty)
        {

            GameObject neighbor = blocks[UnityEngine.Random.Range(0, blocks.Length - 1)];
            normal = (neighbor.transform.position - bplanet.transform.position).normalized;

            Vector3 trig = new Vector3(Mathf.Sin(Time.time)*Mathf.Cos(Time.time*2),Mathf.Cos(Time.time)* Mathf.Cos(Time.time)/2,Mathf.Sin(Time.time/1f)*Mathf.Cos(Time.time*1f)).normalized;
            Vector3 rand = joyridingpoop.randomvec();
            Vector3 randomTangent = Vector3.Cross(normal, rand).normalized;
            newblockLocation = neighbor.transform.position + randomTangent * 1;

            if (snake)
            {
                newblockLocation = last.transform.position + trig* 1;
                break;
            }

            if (UnityEngine.Random.Range(1, 4) == 1)
            {
                newblockLocation = neighbor.transform.position + normal;
               // empty = false;
            }

            if (Physics.OverlapSphere(newblockLocation, 0.55f*checkradius).Length == 0)
                empty = false;




            // add to top of the stack if the space is taken
            if (empty&&tree && UnityEngine.Random.Range(0f,1f)<treeskinny)         
            {
                while (Physics.OverlapSphere(newblockLocation + normal, 0.55f*checkradius).Length != 0)
                {
                    newblockLocation += normal;
                }
                empty = false;
            }

            //only put the block down if its near a bunch of others for dense
            if (dense && UnityEngine.Random.Range(1, 5) >1)
                if (dense && Physics.OverlapSphere(newblockLocation, 0.55f*checkradius).Length < 5)
                    empty = true;

            if (sparse && UnityEngine.Random.Range(1, 5) > 1)
                if (dense && Physics.OverlapSphere(newblockLocation, 0.55f*checkradius).Length > 1)
                    empty = true;


        }

        var b = Instantiate(block, newblockLocation, Quaternion.identity);
        Quaternion q = new Quaternion();
        q.SetFromToRotation(Vector3.up,normal);
        b.transform.rotation = q * transform.rotation;
        b.transform.parent = gameObject.transform;
        if (snake) last = b;
        count++;
    }
}
