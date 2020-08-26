using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    // shootin n tootin
    private Transform gunTip;
    private Vector3 shootDir;       // direction of the shot in world space
    private Transform connection;   // transform of the object we grapple to
    private Vector3 hitPoint;       // collision point of the grapple hook, local to the connection transform
    public float MaxDistance = 1000;
    private float durk = 1;         // a durr
    private bool retracting = false;

    // rope drawin
    public Material ropeMat;
    private LineRenderer rope;
    private float ropeWidth = 0.15f;
    public float shootSpeed = 25;
    private float timeShot = 0;
    private GameObject tipSphere;

    // spring stuff
    private float restDistance; 
    private float SpringK = 20f;
    private float MinRestDistance = 0; // the rest distance for the spring will shrink until it hits this value
    public float MaxSpringForce = 100f;
    public float damper = 0.5f;        // impulse = spring_force - damper * velocity
    public float PullSpeed = 0.02f;    // how quickly the rest distance of the spring will shrink
    

    // Start is called before the first frame update
    void Start()
    {
        gunTip = transform.BFS("PlayerGunBoneEnd"); // search child objects for the tip of the gun

        // rope renderer
        rope = gameObject.AddComponent<LineRenderer>();
        rope.positionCount = 0;
        rope.material = ropeMat;
        rope.startWidth = ropeWidth;

        // hook placeholder
        tipSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer tipRenderer =  tipSphere.GetComponent<Renderer>();
        tipRenderer.enabled = false;
        tipRenderer.material = ropeMat;
        tipSphere.transform.localScale *= 0.5f;
        Destroy( tipSphere.GetComponent<SphereCollider>() );
    }

    // Update is called once per frame
    void Update()
    {
        if ( !retracting && (Input.GetKeyDown(KeyCode.F) ||  timeShot*shootSpeed < 0) ) {
            // save direction of the shot
            shootDir = Camera.main.transform.TransformDirection(Vector3.forward);
        }
        if ( !retracting && !connection && Input.GetKey(KeyCode.F) ) {
            // move the rope & check collisions
            timeShot += Time.deltaTime*durk;
            float distanceTraveled = timeShot*shootSpeed;
            if (distanceTraveled > MaxDistance || distanceTraveled < 0) {
                // flip the rope from retracting/shooting
                durk*=-1;
            }
            
            // cast against everything but the gun projectiles
            int layerMask = 1 << 2;
            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, shootDir, out hit, distanceTraveled, layerMask)) {     
                // save transform & distance of the hit
                connection = hit.transform;
                restDistance = hit.distance;
                hitPoint = connection.InverseTransformPoint(hit.point); // save collision point in local space
                // timeShot = 0;
            } else {
                // update rope positions
                Vector3 ropeTip = transform.TransformPoint(transform.InverseTransformDirection(shootDir)*timeShot*shootSpeed);
                setRopeTip(ropeTip);
            }
        } else if ( Input.GetKeyUp(KeyCode.F) ) {
            // disconnect the rope
            retracting = true;
            connection = null;
            // timeShot = 0;
            // tipSphere.GetComponent<Renderer>().enabled = false;
        } 
        
        if ( retracting ) {
            timeShot -= Time.deltaTime;
            float distanceTraveled = timeShot*shootSpeed;
            if ( distanceTraveled < 0) {
                // stop drawing the rope
                timeShot = 0;
                tipSphere.GetComponent<Renderer>().enabled = false;
                rope.positionCount = 0;
                retracting = false;
            } else {
                Vector3 ropeTip = transform.TransformPoint(transform.InverseTransformDirection(shootDir)*timeShot*shootSpeed);
                setRopeTip(ropeTip);
            }
        }

        if ( connection ) {
            setRopeTip(connection.TransformPoint(hitPoint));  
        } else if (timeShot == 0) {
            rope.positionCount = 0;
        }
    }

    void FixedUpdate() 
    {
        if ( connection ) {
            // get positions for spring force
            float currentDistance = Vector3.Distance(transform.position, connection.TransformPoint(hitPoint));
            restDistance *= (1f-PullSpeed);
            if ( restDistance < MinRestDistance)
                restDistance = MinRestDistance;

            // apply spring force
            Vector3 forceDir = connection.TransformPoint(hitPoint) - transform.position;
            Rigidbody body = gameObject.GetComponent<Rigidbody>();
            Vector3 spring = forceDir.normalized * SpringK * (currentDistance - restDistance);
            Vector3 impulse = spring - damper*body.velocity;
            body.AddForce( impulse.magnitude < MaxSpringForce ? impulse : impulse.normalized * MaxSpringForce);
        }
    }

    private void setRopeTip(Vector3 worldSpacePoint) {
        rope.positionCount = 2;
        rope.SetPosition(0, gunTip.position);
        rope.SetPosition(1, worldSpacePoint);   
        tipSphere.transform.position = worldSpacePoint;
        tipSphere.GetComponent<Renderer>().enabled = true;
    }

    // calling this from the color change script to avoid random public variables
    public void setColor(Color color) {
        rope.material.SetColor("_Color", color);
        tipSphere.GetComponent<Renderer>().material.SetColor("_Color", color);
    }
} 
