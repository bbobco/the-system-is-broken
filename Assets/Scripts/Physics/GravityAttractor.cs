using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class GravityAttractor : MonoBehaviour {
	
	public static float gravity = 100; // gravity per update is 700*mass of the attractor
    public float smooth = 0.5F;
    public float gmass; // represents mass of object in terms of its gravitational pull
    public bool primary = false;
    public bool justchanged = false;
    public float stupidscale = 1;
    public static float distPow = 1.4f;
    float timesincechanged = 0;


    void Awake()
    {
        /*
        int spinspeed = UnityEngine.Random.Range(0, 10);
        Vector3 randomspin = new Vector3(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50)).normalized;
        this.GetComponent<Rigidbody>().AddTorque(randomspin * spinspeed*GetComponent<Rigidbody>().mass, ForceMode.VelocityChange);
        */

        var fpsController = GameObject.Find("player").GetComponent<FirstPersonController>();
        // g = -2h/t^2
        // gravity = 10*fpsController.JumpHeight / fpsController.JumpTimeToPeak; 
    }

    public void Attract(Rigidbody body) {

         //UnityEngine.Debug.Log("dsfsds dfsfdfdss "+ body.gameObject.transform.position);
        Vector3 gravityUp = (body.gameObject.transform.position - transform.position).normalized;

        float dist = (body.gameObject.transform.position - transform.position).magnitude;

        dist /= 10;
      // UnityEngine.Debug.Log("ass but " + gravityUp);

        Vector3 localUp = body.gameObject.transform.up;
        /*
        if(primary)
            body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
        */
        //TEMPORARY pls fix    
        if (body.gameObject.name.Equals("evildoer"))
        {
            Quaternion wantedRotation = Quaternion.FromToRotation(localUp, gravityUp);
            body.rotation = Quaternion.Lerp(body.rotation, wantedRotation * body.rotation, .02f * 5.5f);
        }

        //*****************************************************

        //only for player below
        //UnityEngine.Debug.Log(primary);
        float rotspeed = 4.5f;
        if (primary && body.gameObject.name.Equals("player"))
        {
            float distToRotate = 40f;
            float distFromGround = (body.gameObject.transform.position - transform.position).magnitude - transform.localScale.x / 2*stupidscale;
          //      UnityEngine.Debug.Log("drggggg " + distFromGround);
            if (distFromGround < distToRotate) { 
                Quaternion wantedRotation = Quaternion.FromToRotation(localUp, gravityUp);//Quaternion.LookRotation(body.gameObject.transform.position - transform.position);
                body.rotation = Quaternion.Lerp(body.rotation, wantedRotation * body.rotation, .02f * rotspeed);
            }
     //       UnityEngine.Debug.Log(FirstPersonController.cameraTransform.right);//"dur " + GameObject.Find("Player").GetComponent<FirstPersonController>());//.cameraTransform.right);
            if (!justchanged && distFromGround < distToRotate)
            {
                timesincechanged += .02f;
                if (timesincechanged > .5f)
                {
                    justchanged = false;
                    timesincechanged = 0;
                }
                var fpsController = body.gameObject.GetComponent<FirstPersonController>();
                fpsController.verticalLookRotation *= .95f;
                if (fpsController.verticalLookRotation > 0) fpsController.verticalLookRotation-=2;
                else fpsController.verticalLookRotation+=2;
                //FirstPersonController.verticalLookRotation*=.95f;
            }
        }


        // Apply downwards gravity to body
        //float grav = 700;
        //    UnityEngine.Debug.Log(body.gameObject.name);
        float distScale = Mathf.Pow(dist, -distPow);   // lmao
        body.AddForce(gravity*-gravityUp *distScale*gmass*body.mass);

       // body.AddForce(Vector3.left*12412f);
        // Allign bodies up axis with the centre of planet

        //bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, 50.1f);

        //if (IsGrounded)
        //{
        //    Vector3 TargetRotation = Vector3.RotateTowards(localUp, gravityUp, smooth * Time.deltaTime, 0F);

        //    body.rotation = Quaternion.FromToRotation(localUp, TargetRotation) * body.rotation;
        //}


        // body.rotation = Quaternion.RotateTowards(localUp,gravityUp, smooth * Time.deltaTime) * body.rotation;
    }
    public float dist(Rigidbody body)
    {
        // lol
        return (body.gameObject.transform.position - transform.position).magnitude;
    }
}
