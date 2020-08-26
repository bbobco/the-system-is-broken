using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {
	
	public static GravityAttractor[] attractors = null;
	Rigidbody MyRigidBody;

	public bool immune = false;
	public GravityAttractor winner = null;


	void Awake () {

		if (attractors == null)
		{
			GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
			attractors = new GravityAttractor[planets.Length];
			for (int i = 0; i < attractors.Length; i++)
			{
				attractors[i] = planets[i].GetComponent<GravityAttractor>();
			}
		}

		MyRigidBody = GetComponent<Rigidbody>();

		// Disable MyRigidBody gravity and rotation as this is simulated in GravityAttractor script
		MyRigidBody.useGravity = false;
		MyRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
	void FixedUpdate () {
		//GameObject player = GameObject.Find("Player");
		if (winner == null) winner = GameObject.Find("player").GetComponent<GravityBody>().winner;
		if (MyRigidBody.gameObject.name.Equals("player") || MyRigidBody.gameObject.tag.Equals("enemy") || MyRigidBody.gameObject.tag.Equals("block")||MyRigidBody.gameObject.tag.Equals("asteroid")) //temporary for AI 
		{
			float dist = 0;
			
			int count = 0;
			foreach (GravityAttractor attractor in attractors)
			{
				if (attractor == null) continue;
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

			foreach (GravityAttractor attractor in attractors)
			{
				if (attractor == null) continue;
				if (attractor != winner) attractor.primary = false;
			}
			//UnityEngine.Debug.Log(count);
			if(winner!=null)
				if (!winner.primary) winner.justchanged = true;
					winner.primary = true;
		}


		if (!gameObject.GetComponent<Rigidbody>().useGravity&&!immune)
		{
			//UnityEngine.Debug.Log("dfsdggd");
			// Allow this body to be influenced by planet's gravity
			foreach (GravityAttractor attractor in attractors)
			{
				//UnityEngine.Debug.Log(gameObject.name);
				if (attractor == null) continue;
				if (attractor.gameObject.name.Equals(gameObject.name)) continue;
				attractor.Attract(MyRigidBody);
			}
		}
	}
}