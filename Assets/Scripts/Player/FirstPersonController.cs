using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class FirstPersonController : MonoBehaviour {

	// camera stuff
	public float mouseSensitivityX = 1f;
	public float mouseSensitivityY = 1f;
	public float verticalLookRotation;
	private Transform cameraTransform;	

	// horizontal movement
	public float walkSpeed = 6;
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;

	// jump
	public float jumpForce = 1000;
	private bool grounded;

	// dash ability
	public float DashSpeed = 100;
	public float DashDuration = 0.5f;
	public float DashCD = 1;
	private Vector3 dashDirection;
	private bool dashing = false;
	private bool inDashCD = false;
	private GameObject planet;

	// bullshit
	Rigidbody playerBody;
	public bool dead = false;

	void Awake() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		cameraTransform = Camera.main.transform;
		
		// force the main camera to be chosen 4 dummies
		foreach ( Camera cam in Camera.allCameras ) {
			cam.gameObject.SetActive(false);
		}
		cameraTransform.gameObject.SetActive(true);

		playerBody = GetComponent<Rigidbody> ();
        grounded = true;

		planet = GameObject.FindGameObjectsWithTag("Planet")[0];

	}

	void Update() {
		if (dead) return;

		if ( dashing ) {


			// dash toward the camera forward
			// GetComponent<Rigidbody>().velocity = cameraTransform.forward * DashSpeed;

			// dash towards a saved input
			// dashDirection = Maff.FastestPathOnSphere(dashDirection, (transform.position - planet.transform.position).normalized );
			// GetComponent<Rigidbody>().velocity = dashDirection.normalized * DashSpeed;
			//playerBody.MovePosition(playerBody.position +  Maff.FastestPathOnSphere(dashDirection, transform.up) * DashSpeed * Time.deltaTime);
			// transform.position += Maff.FastestPathOnSphere(transform.right.normalized, (transform.position - GetComponent<GravityBody>().winner.transform.position).normalized) * 15 * Time.fixedDeltaTime;
		
			Vector3 solution = chompy.FastestPathOnSphere(dashDirection.normalized, (transform.position - GetComponent<GravityBody>().winner.transform.position).normalized) * Time.fixedDeltaTime;
            transform.position += solution * 80;

			Vector3 rotation = Vector3.zero;
			if (Vector3.Dot(dashDirection, transform.forward) > .9)
				rotation = transform.forward;
			if (Vector3.Dot(dashDirection, transform.right) > .9)
				rotation = transform.right;
			if (Vector3.Dot(dashDirection, -transform.right) > .9)
				rotation = -transform.right;
			if (Vector3.Dot(dashDirection, -transform.forward) > .9)
				rotation = -transform.forward;



			Quaternion q = new Quaternion();
            q.SetFromToRotation(rotation,solution.normalized);
            q = q * transform.rotation;
            transform.rotation = q;
		}
			

		// Look rotation:
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X")  * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis("Mouse Y")  * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation,-80,80);
		cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

		// horizontal movement
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		Vector3 moveDir = new Vector3(inputX,0, inputY).normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount,targetMoveAmount,ref smoothMoveVelocity,.15f);

		// jump
		if (Input.GetButtonDown("Jump") && grounded) {
			playerBody.AddForce(transform.TransformDirection(Vector3.up)*jumpForce);
		}

		// dash 
		if(Input.GetKey("left shift") && !dashing && !inDashCD ) {	
			// open another thread to time the dash
			StartCoroutine(startDash(moveDir));
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

		// casting against everything but the gun projectiles
		int layerMask = 1 << 2;
		layerMask = ~layerMask;
		// cast down to the bottom of the player collider
		RaycastHit durk;
		CapsuleCollider cc = GetComponent<CapsuleCollider>();
		if ( Physics.Raycast( transform.position, transform.TransformDirection(Vector3.down), 
								out durk, (cc.height*0.5f+cc.radius-cc.center.y)*transform.localScale.y, layerMask)) {     
			grounded = true;
		} else {
			grounded = false;
		}
	}

	// wait on dash timers
	private IEnumerator startDash(Vector3 moveDir) {
		dashing = true;
		dashDirection = transform.TransformDirection(moveDir);

		// wait for dash to complete
		yield return new WaitForSeconds(DashDuration);

		// set dash complete
		dashing = false;
		inDashCD = true;
		GetComponent<Rigidbody>().velocity = Vector3.zero;

		// wait for dash cooldown
		yield return new WaitForSeconds(DashCD);
	
		inDashCD = false;
	}
	
	Vector3 lastTimePosition = Vector3.zero;
	Vector3 deadforward = Vector3.zero;
	Vector3 deaddown = Vector3.zero;
	Vector3 deadright = Vector3.zero;
	public float timeOfDeath = 0;
	public Vector3 currentVelocity; // needed for bullets to start with our velocity, since we dnt technically have one because we just change the position
	void FixedUpdate() {
		// Apply movement to playerBody	
		if(dead)
        {

			if (Time.time - timeOfDeath > 1.2f)
			{
				
					Quaternion qr = new Quaternion();
				qr.SetLookRotation(transform.up, transform.right);
				qr = qr * transform.rotation;
				transform.rotation = qr;
				return;
			}
			//if (deadforward.x==0 &&deadforward.y==0&&deadforward.z==0) deadforward = transform.forward;
			if (deadright.x==0 &&deadright.y==0&&deadright.z==0) deadright = transform.right;
		    if (deaddown.x==0 &&deaddown.y==0&&deaddown.z==0) deaddown = -transform.up;
			Vector3 normal = transform.position-GetComponent<GravityBody>().winner.transform.position;
			Vector3 goaldir = Vector3.Cross(normal,deadforward);
			/*
			if (Vector3.Dot(normal, goaldir) > .8)
				return;
				*/
	
			Quaternion q = new Quaternion();
			q.SetLookRotation(deaddown, deadright);
			q = q * transform.rotation;

			if (Time.time - timeOfDeath < 1.2f)
				transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 1); //Quaternion.Slerp(transform.rotation, q, Time.fixedDeltaTime * 1f);

			return;
		}

		//UnityEngine.Debug.Log("playerbody velocity " + playerBody.velocity);
		Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime  *2;
		//	currentVelocity = localMove / Time.fixedDeltaTime  + playerBody.velocity;
		playerBody.MovePosition(playerBody.position + localMove);

		currentVelocity = (playerBody.position - lastTimePosition)/Time.fixedDeltaTime;
		lastTimePosition = playerBody.position;
	}

	public bool getDashCD() {
		return inDashCD;
	}
}

