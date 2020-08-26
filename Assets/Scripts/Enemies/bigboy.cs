using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigboy : MonoBehaviour
{
    float slowThreshold = 120;
    public bool deathrayActive = !false;
    float deathStartTime = 0;
    public GameObject fire;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        slowDownPlayer();
      if(deathrayActive)
          deathRay();
          */
    }

    void slowDownPlayer()
    {
        GameObject player = GameObject.Find("player");
        if((transform.position-player.transform.position).magnitude < slowThreshold)
        {
            player.transform.position -= player.GetComponent<FirstPersonController>().currentVelocity * Time.fixedDeltaTime / 2;
        }

    }

    int numeffects=0;
    void deathRay()
    {
        UnityEngine.Debug.Log("deathray doin stuff");
        if (deathStartTime == 0) deathStartTime = Time.time -1;
        float rotspeed = (Time.time - deathStartTime);
        transform.Rotate(new Vector3(0,(int)rotspeed,0), Space.World);

        if(Time.time-deathStartTime>numeffects)
        {
            Quaternion q = new Quaternion();
            q.SetFromToRotation(transform.forward, -transform.up);
            q = q * transform.rotation;
    //        var p = Instantiate(fire, transform.position-transform.up*50+joyridingpoop.randomvec()*10, q);
       //     var d = Instantiate(explosion, transform.position, q);
            //Destroy(p, 2.5f);
            numeffects++;
        }
    }

}
