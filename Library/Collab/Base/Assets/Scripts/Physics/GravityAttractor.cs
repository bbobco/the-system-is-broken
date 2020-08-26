using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	
	public float gravity = -9.8f;
    public float smooth = 0.5F;


    public void Attract(Rigidbody body) {
		Vector3 gravityUp = (body.gameObject.transform.position - transform.position).normalized;

        const float scalereduction = 1 / 10.0f;
        float distancegravityscale = (body.gameObject.transform.position - transform.position).magnitude*scalereduction;


        Vector3 localUp = body.transform.up;

       // body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;

        // Apply downwards gravity to body
        body.AddForce(gravityUp * gravity/distancegravityscale/distancegravityscale);
        // Allign bodies up axis with the centre of planet

        //bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, 50.1f);

        //if (IsGrounded)
        //{
        //    Vector3 TargetRotation = Vector3.RotateTowards(localUp, gravityUp, smooth * Time.deltaTime, 0F);

        //    body.rotation = Quaternion.FromToRotation(localUp, TargetRotation) * body.rotation;
        //}


        // body.rotation = Quaternion.RotateTowards(localUp,gravityUp, smooth * Time.deltaTime) * body.rotation;
    }  
}
