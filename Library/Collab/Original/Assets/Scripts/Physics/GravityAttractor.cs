using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class GravityAttractor : MonoBehaviour {
	
	public float gravity = -9.8f;
    public float smooth = 0.5F;
    public float gstrength;
    public bool primary = false;

    public void Attract(Rigidbody body) {
		Vector3 gravityUp = (body.gameObject.transform.position - transform.position).normalized;

        float scalereduction = 1 / gstrength;
        float dist = (body.gameObject.transform.position - transform.position).magnitude;

        dist /= 10;
       // UnityEngine.Debug.Log(gravityUp);

        Vector3 localUp = body.transform.up;
        /*
        if(primary)
            body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
        */
        float rotspeed = 1.5f;
        if (primary)
        {
            float distFromGround = (body.gameObject.transform.position - transform.position).magnitude - transform.localScale.x;
            if (distFromGround < 3) { 
                Quaternion wantedRotation = Quaternion.FromToRotation(localUp, gravityUp);//Quaternion.LookRotation(body.gameObject.transform.position - transform.position);
                body.rotation = Quaternion.Lerp(body.rotation, wantedRotation * body.rotation, Time.deltaTime * rotspeed);
            }
        }
            

        // Apply downwards gravity to body
        body.AddForce(700*-gravityUp *1/dist/dist);

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

        return (body.gameObject.transform.position - transform.position).magnitude;
    }
}
