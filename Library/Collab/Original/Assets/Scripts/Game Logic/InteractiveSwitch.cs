using UnityEngine;

public class InteractiveSwitch : MonoBehaviour
{

    public Material mat;

    private void Start()
    {
        mat = Resources.Load("Materials/Dissolve") as Material;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Bit shift the index of the layer (8) to get a bit mask
            //int layerMask = 1 << 8;

            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, fwd, out hit, 10) && hit.transform.tag == "Switch")
            {
                print("Switch interaction!!!!1!");
                //hit.rigidbody.gameObject.GetComponent<Renderer>().material = mat;

               Animator anim = hit.rigidbody.gameObject.GetComponent<Animator>();
                anim.Play("Flip");
                gameObject.GetComponentInParent<Rigidbody>().useGravity = true;
            }
                
        }  
    }
}