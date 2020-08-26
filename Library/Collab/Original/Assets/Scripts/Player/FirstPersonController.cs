using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class FirstPersonController : MonoBehaviour {

	// public vars
	public bool dashing = false;
	public float dashtime = 0;
	public float mouseSensitivityX = 1f;
	public float mouseSensitivityY = 1f;
	public float walkSpeed = 6;
	public float jetpackForce = 500;
	public float jumpForce = 1000;
	public LayerMask groundedMask;
	public bool useJetpack = false;
	public AudioSource jetpackSound;
	public bool jetpackAccelerating;
	public bool jetpackDecelerating;
	public int jetpackDecelerateTimer;
	private float maxJetPackSpeed;
	public bool grounded;
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	public static float verticalLookRotation;
	public static Transform cameraTransform;
	Rigidbody playerBody;
	public bool dead = false;

	private JetBar jetBar;

	public GameObject asteroid;
	
	
	void Start()
    {	
		// search children for hud
		jetBar = transform.BFS("JetBar").GetComponent<JetBar>();
	}

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
		//jetpackSound = gameObject.GetComponent<AudioSource>();

		if ( useJetpack ) {
			jetpackSound = gameObject.AddComponent<AudioSource>();
			jetpackSound.clip = Resources.Load("Audio/JetPackSound") as AudioClip;
			jetpackAccelerating = false;
			jetpackDecelerating = true;
			maxJetPackSpeed = 4;
		}
	}

	float timeLastUsedJetpack = 0;
	void Update() {
		if (dead) return;

		if(Time.time-.3f>dashtime&&dashing)
        {

			GetComponent<Rigidbody>().velocity = Vector3.zero;
			dashing = false;
		}
		// Look rotation:
		
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X")  * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis("Mouse Y")  * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation,-80,80);
		//UnityEngine.Debug.Log(verticalLookRotation);
		cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
		
		/*
		transform.Rotate(transform.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
		cameraTransform.localEulerAngles = transform.right*-1 * verticalLookRotation;
		*/

		// Calculate movement:
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		
		Vector3 moveDir = new Vector3(inputX,0, inputY).normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount,targetMoveAmount,ref smoothMoveVelocity,.15f);

		// Jump
		//UnityEngine.Debug.Log((JetBar.GetJetBarValue()));
		if (Input.GetButtonDown("Jump") && (jetBar.GetJetBarValue() > 0) && useJetpack) 
		{
			jetpackSound.Play();
			jetpackAccelerating = true;
			jetpackDecelerating = false;
			timeLastUsedJetpack = Time.time;

		}
		if (Input.GetButtonDown("Jump") && grounded) {
			Debug.Log("jump input");
			playerBody.AddForce(transform.TransformDirection(Vector3.up)*jumpForce);
		}
		if (Input.GetButton("Jump"))
		{
			jetBar.SetJetBarValue(jetBar.GetJetBarValue() - .004f*0.75f);
		}
		if (Input.GetButtonUp("Jump") || (jetBar.GetJetBarValue()==0))
		{
			
			if ( useJetpack ) {
				jetpackAccelerating = false;
				jetpackDecelerating = true;
				jetpackDecelerateTimer = 8;
				jetpackSound.Stop();
			} 
			/*
			if (gameObject.GetComponent<PlayerPhysics>() != null)
				gameObject.GetComponent<PlayerPhysics>().RotateTowardsGravity(-55f);
				*/
		}

		if(Input.GetKey("left shift"))
        {
			Vector3 solution = chompy.FastestPathOnSphere(transform.right.normalized, (transform.position - GetComponent<GravityBody>().winner.transform.position).normalized) * Time.fixedDeltaTime;
			transform.position += solution * 80;

			Quaternion q = new Quaternion();
			q.SetFromToRotation(transform.right,solution.normalized);
			q = q * transform.rotation;
			transform.rotation = q;
		//	GetComponent<Rigidbody>().velocity = cameraTransform.forward * 80;
			dashing = true;
			dashtime = Time.time;
        }
        // quit
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

		// cast against everything but the gun projectiles
		int layerMask = 1 << 2;
		layerMask = ~layerMask;
		RaycastHit durk;
		CapsuleCollider cc = GetComponent<CapsuleCollider>();
		if ( Physics.Raycast( transform.position, transform.TransformDirection(Vector3.down), 
								out durk, (cc.height*0.5f+cc.radius)*transform.localScale.y, layerMask)) {     
			grounded = true;
		} else {
			grounded = false;
		}
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

		if (jetpackAccelerating && useJetpack)
		{
			
			playerBody.AddForce(transform.up * jetpackForce);
			//JetBar.SetJetBarValue(JetBar.GetJetBarValue() - .005f);
			float currentSpeed = playerBody.velocity.magnitude;
			if (currentSpeed > maxJetPackSpeed)
			{
				playerBody.velocity = playerBody.velocity.normalized * maxJetPackSpeed;
			}
		}
		/*
		if (jetpackDecelerating)
		{
			playerBody.AddForce(transform.up * -16);
			jetpackDecelerateTimer--;
			if (jetpackDecelerateTimer == 0)
			{
				jetpackDecelerating = false;
				jetpackForce = 50;	
			}
		}
		*/

		//UnityEngine.Debug.Log("playerbody velocity " + playerBody.velocity);
		Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime  *2;
	//	currentVelocity = localMove / Time.fixedDeltaTime  + playerBody.velocity;
		playerBody.MovePosition(playerBody.position + localMove);

		currentVelocity = (playerBody.position - lastTimePosition)/Time.fixedDeltaTime;
		lastTimePosition = playerBody.position;

		//UnityEngine.Debug.Log("aaa" + playerBody.position + " " + transform.position);
		if ( useJetpack ) {
			float rechargeTimer = 3.5f;
			// Replenish jet bar and jet bar not full
			//Time.time-timeLastUsedJetpack > rechargeTimer && 
			if (Time.time - timeLastUsedJetpack > rechargeTimer && !jetpackAccelerating && jetBar.GetJetBarValue() <= .995F)
				jetBar.SetJetBarValue(jetBar.GetJetBarValue() + .005f);
		}
	}
}
