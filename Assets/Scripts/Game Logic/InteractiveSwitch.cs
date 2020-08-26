using UnityEngine;

public class InteractiveSwitch : MonoBehaviour
{

    public Material mat;
    private bool gravityFlip;
    private Transform cameraTransform;
    private GameObject player;
    private Rigidbody body;
    private Quaternion targetRotation;
    private bool doneRotating = false;

    public float rotationSpeed = 2f;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        player = GameObject.Find("player");
        body = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 fwd = cameraTransform.TransformDirection(Vector3.forward);

            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, fwd, out hit, 10) && hit.transform.tag == "Switch")
            {
                print("Switch interaction!!!!1!");

               Animator anim = hit.collider.gameObject.GetComponent<Animator>();
                gravityFlip = !gravityFlip;
               anim.SetBool("Flip", gravityFlip);
 			   player.GetComponent<Rigidbody>().useGravity = true;
               AudioSource leverSound = gameObject.GetComponent<AudioSource>();
               leverSound.Play();

                if (gravityFlip)
                    targetRotation = Quaternion.FromToRotation(body.transform.up, new Vector3(0.0f, 1.0f, 0.0f)) * body.rotation;
            }
               
        }  
    }

    private void FixedUpdate()
    {
        if (gravityFlip && body.transform.up != Vector3.up)
        {
            body.rotation = Quaternion.RotateTowards(body.rotation, targetRotation, rotationSpeed);
            //if ( body.rotation.eulerAngles.y == ta)
        }
    }
}