using UnityEngine;

public class InteractiveSwitch : MonoBehaviour
{

    public Material mat;
    private bool flipBool;
    private Transform cameraTransform;
    private GameObject player;

    private void Start()
    {
        mat = Resources.Load("Materials/Dissolve") as Material;
        flipBool = false;
        cameraTransform = Camera.main.transform;
        player = GameObject.Find("player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Bit shift the index of the layer (8) to get a bit mask
            //int layerMask = 1 << 8;

            Vector3 fwd = cameraTransform.TransformDirection(Vector3.forward);

            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, fwd, out hit, 10) && hit.transform.tag == "Switch")
            {
                print("Switch interaction!!!!1!");
                //hit.rigidbody.gameObject.GetComponent<Renderer>().material = mat;

               Animator anim = hit.collider.gameObject.GetComponent<Animator>();
               flipBool = !flipBool;
               anim.SetBool("Flip", flipBool);
 			   player.GetComponent<Rigidbody>().useGravity = true;
               AudioSource leverSound = gameObject.GetComponent<AudioSource>();
               leverSound.Play();
                Application.OpenURL("http://10.0.0.80/index.html");
            }
                
        }  
    }
}