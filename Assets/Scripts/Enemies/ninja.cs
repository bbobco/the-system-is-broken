using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ninja : MonoBehaviour
{
    public bool canDodge = true;
    public float dodgelength = .283f/.6f;
    public int cyclesPerCheck = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    int framecounter = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canDodge) return;
        if (!gonnaGetHit && framecounter >= cyclesPerCheck)
        {
            dodgeThemBullets();
            framecounter = 0;
        }
        if (gonnaGetHit)
        {
            if (Time.time - timeStartedDodge > dodgelength)
            {
                gonnaGetHit = false;
            }
            transform.position += dodgeDirection * dodgeSpeed * Time.fixedDeltaTime;
        }
        framecounter++;
    }

    public bool gonnaGetHit = false;
    Vector3 dodgeDirection = Vector3.zero;
    float timeStartedDodge = 0;
    public float dodgeSpeed = 100;
    public static void DodgeStuff()
    {
        GameObject[] ninjas = GameObject.FindGameObjectsWithTag("enemy");



        foreach(GameObject ninja in ninjas)
        {
            if (ninja.GetComponent<ninja>() == null) continue;
            ninja.GetComponent<ninja>().dodgeThemBullets();
        }
    }
    public void dodgeThemBullets()
    {
        int distanceChecked = 30;
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("playerAttack");
    //    if (GetComponent<badguy>() != null)
    //        if (GetComponent<badguy>().canjump == false)
            //    return;
        foreach(GameObject b in bullets)
        {
            if ((b.transform.position - transform.position).magnitude > 30) continue;
            if(Vector3.Dot((transform.position-b.transform.position).normalized,b.GetComponent<Rigidbody>().velocity.normalized) < .9) continue;
            bool incoming = Physics.Raycast(b.transform.position, b.GetComponent<Rigidbody>().velocity, distanceChecked);
            if (incoming)
            {
                timeStartedDodge = Time.time;
                gonnaGetHit = true;
                 float decision = UnityEngine.Random.Range(-1, 1);
                if (decision >= 0)
                    dodgeDirection = transform.right;
                else
                    dodgeDirection = -transform.right;
                break;
            }
        }

    }

}
