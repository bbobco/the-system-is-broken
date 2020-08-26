using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class turret : MonoBehaviour
{
    public GameObject target;
    bool activated = false;
    public GameObject bullet;
    Animator animator;
    float shootTimer=0;
    float shotRate = .25f;
    public GameObject gunFlash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!activated && (transform.position - target.transform.position).magnitude < 45)
        {
            activated = true;
            animator.SetBool("Active", true);
        }
        if (!activated) return;

        pointAtTarget();
        if(Time.time-shootTimer > shotRate )
        {
            shootTimer = Time.time;
            Vector3 bulletStart = GameObject.Find("Canon_Spout").transform.position;
            Instantiate(bullet,bulletStart, Quaternion.identity);
        }


        //     Quaternion fix = Quaternion.Euler(0, 0, 45);
        //    fix = fix * Quaternion.identity;
        // fix = fix * 

        //  fix = fix*q;


        //




    }

    // what a MESS FJFKASFDFAKFKJDKSD
    void pointAtTarget()
    {

        Vector3 dir = (transform.position - target.transform.position).normalized;

        Transform rt = transform.GetChild(0).GetChild(2);

        Vector3 xzLook = chompy.FastestPathOnSphere(dir, rt.up);
        Quaternion q = new Quaternion();

        q.SetFromToRotation(rt.forward, xzLook);
        q = q * rt.rotation;
        rt.rotation = Quaternion.Slerp(rt.rotation, q, 1f);


        rt.Rotate(0, -90, 0, Space.Self);
        Transform ud = rt.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0);

        //   UnityEngine.Debug.Log(ud.gameObject.name);

        q = new Quaternion();
        Vector3 yLook = chompy.FastestPathOnSphere(-dir, ud.up);
        q.SetFromToRotation(-ud.right, yLook);
        q = q * ud.rotation;
        ud.rotation = Quaternion.Slerp(ud.rotation, q, 1f);
    }

}
