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
		//GameObject player = GameObject.Find("Player");
		float dist = 0;
		GravityAttractor winner = null;
		int count = 0;
		foreach (GravityAttractor attractor in attractors)
		{
			//UnityEngine.Debug.Log("ass");
			count++;
//			attractor.primary = false;
			float tdist = attractor.dist(MyRigidBody);
			//UnityEngine.Debug.Log(count + "  " + tdist);
			if (tdist < dist || dist == 0)
			{
				winner = attractor;
				dist = tdist;
			}
		}
		foreach (GravityAttractor attractor in attractors) if (attractor != winner) attractor.primary = false;
			//UnityEngine.Debug.Log(count);
		if (!winner.primary) winner.justchanged = true;
		winner.primary = true;
		if (!gameObject.GetComponent<Rigidbody>().useGravity)
		{
			//UnityEngine.Debug.Log("dfsdggd");
			// Allow this body to be influenced by planet's gravity
			foreach (GravityAttractor attractor in attractors)
				attractor.Attract(MyRigidBody);
		}
	}
}