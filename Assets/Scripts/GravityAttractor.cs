using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	
	public float gravity = -9.8f;
    public float smooth = 0.5F;


    public void Attract(Rigidbody body) {
		Vector3 gravityUp = (body.position - transform.position).normalized;

		Vector3 localUp = body.transform.up;
		
		// Apply downwards gravity to body
		body.AddForce(gravityUp * gravity);
        // Allign bodies up axis with the centre of planet

        Vector3 TargetRotation = Vector3.RotateTowards(localUp, gravityUp, smooth * Time.deltaTime, 0F);

		body.rotation = Quaternion.FromToRotation(localUp,TargetRotation) * body.rotation;
		// body.rotation = Quaternion.RotateTowards(localUp,gravityUp, smooth * Time.deltaTime) * body.rotation;
	}  
}
