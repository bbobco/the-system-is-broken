using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {
	
	GravityAttractor[] attractors;
	Rigidbody MyRigidBody;
	
	void Awake () {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");

        attractors = new GravityAttractor[planets.Length];

        for ( int i = 0; i < attractors.Length; i++)
        {
            attractors[i] = planets[i].GetComponent<GravityAttractor>();
        }

		MyRigidBody = GetComponent<Rigidbody>();

		// Disable MyRigidBody gravity and rotation as this is simulated in GravityAttractor script
		MyRigidBody.useGravity = false;
		MyRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
	void FixedUpdate () {
		GameObject player = GameObject.FindWithTag("Player");
		if (!player.GetComponent<Rigidbody>().useGravity)
		{
			// Allow this body to be influenced by planet's gravity
			foreach (GravityAttractor attractor in attractors)
				attractor.Attract(MyRigidBody);
		}
	}
}