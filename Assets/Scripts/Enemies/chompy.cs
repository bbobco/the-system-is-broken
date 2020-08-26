using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chompy : MonoBehaviour
{
    public int hp = 5;
    float cooldownTimer = 7;
    float lastAttack = -13;
    // Start is called before the first frame update
    Animator animator;
    bool walking = true;
    bool running = false;
    bool justhit = false;
    bool huntingPlayer = true;
    bool dead = false;
    float fleeTime = 0;
    bool flee = false;
    public float fleeLength = 3;
    bool grounded = true;
    bool jumping = false;
    Vector3 fleeLocation;
    Vector3 targetloc;
    Vector3 lastNormal;
    float attackstart = 0;
    GameObject daddy;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dead) return;
        animator.SetBool("Flee", flee);
        if (GetComponent<ninja>().gonnaGetHit && GetComponent<ninja>().canDodge)
        {
           
            animator.SetBool("Dodge", true);
            return;
        }
        else
            animator.SetBool("Dodge", false);
        // if(animator.GetBool("AttackReady")) animator.SetBool("AttackReady", false);
        GameObject player = GameObject.Find("player");
       // GravityAttractor daddy = GetComponent<GravityBody>().winner;
        daddy = closestPlanet();
        Quaternion q = new Quaternion();


        //if shot OR if attack is on cooldown and we are right next to player
        if (justhit && animator.GetBool("Hit") == false || ((transform.position - player.transform.position).magnitude < 7 && Time.time -lastAttack < cooldownTimer ))
        {
            // UnityEngine.Debug.Log("brody");
            huntingPlayer = false;
            fleeTime = Time.time;
            justhit = false;
            flee = true;
            animator.SetBool("Run", true);
            running = true;
            fleeLocation = joyridingpoop.randomvec() * (daddy.transform.localScale.x / 2 + 2) + daddy.transform.position;
        }
        
        /*
        if (daddy.gameObject.transform.localScale.x / 2 + 3 > (transform.position - daddy.transform.position).magnitude && grounded)
            transform.position += -transform.up * 0.4f;
        */


        if (flee)
        {
            if (Time.time - fleeTime > fleeLength || (transform.position-fleeLocation).magnitude<5)
            {
                
                flee = false;
                huntingPlayer = true;
                if (hp > 2)
                {
                    animator.SetBool("Run", false);
                    running = false; walking = true;
                }          
            }
            else
                targetloc = fleeLocation;
        }
        if (huntingPlayer)
        {
            targetloc = player.transform.position;
        }

        
        float slerprate = 0.09f; // this determines the rate at which SLERRRRRRRRP
        /*
        if (running) slerprate *= 2; //this is because if you are running, you need more SLERP
        Vector3 normal = NormalFromRayCast();
        Vector3 slerpNormal = Vector3.Slerp(lastNormal,normal,slerprate);
        lastNormal = slerpNormal;
        */


        Vector3 directlook = (targetloc- transform.position).normalized;
        Vector3 normal = (transform.position - daddy.transform.position).normalized;
        Vector3 levelToGroundLook = FastestPathOnSphere(directlook, normal);

        
        q.SetFromToRotation(transform.forward, levelToGroundLook);
        q = q * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, slerprate);

        float distFromGround = getRayCastDistFromGround();
        
        if (distFromGround > 0.2)
            transform.position += -transform.up * 0.05f;
        /*
        if (distFromGround < 0f) transform.position += transform.up * 0.05f;
        if (distFromGround > 0.2)
            transform.position += -transform.up * 0.35f;
            */



        //straighten out crooked

        
        if (Vector3.Dot(transform.up,normal)<.997f)
        {
            if (!animator.GetBool("AttackReady"))
            {
                // UnityEngine.Debug.Log("fixing the up");
                Vector3 fixmyup = FastestPathOnSphere(normal, levelToGroundLook);
                Quaternion fix = new Quaternion();
                fix.SetFromToRotation(transform.up, fixmyup);
                fix = fix * transform.rotation;
                fix = Quaternion.Slerp(transform.rotation, fix, slerprate); // this is the 3rd Slerp. Is it really necessary? YES
                transform.rotation = fix;
            }
           // transform.rotation = Quaternion.Slerp(transform.rotation, fix, .09f);
        }
        
       

        // upcomingdir = pathingOnSphere(transform.position - daddy.transform.position, transform.position, player.transform.position, arbloc);
        int moveSpeed = 4;
        if (walking) moveSpeed = 7;
        if (running) moveSpeed = 31 ;
        if (Time.time - fleeTime < .75f)
            moveSpeed = 0;
        float timeSinceAttackStart = Time.time - attackstart;
        if (animator.GetBool("AttackReady") && timeSinceAttackStart<.80f)
            moveSpeed = 0;
        if (animator.GetBool("AttackReady") && timeSinceAttackStart > .80f)
        {
            moveSpeed = 19;
        }

        transform.position += transform.forward * Time.fixedDeltaTime * moveSpeed;

        Vector3 dir = (player.transform.position-transform.position);
        if (dir.magnitude < 15 && Time.time-lastAttack>cooldownTimer )
        {
            
            float dot = Vector3.Dot(dir.normalized,transform.forward);
            if (dot > .8f)
            {
                lastAttack = Time.time;
                animator.SetBool("AttackReady", true);
                attackstart = Time.time;
            }
        }
        
    }


   public static Vector3 FastestPathOnSphere(Vector3 look, Vector3 normal)
    {
        Vector3 intermediate = Vector3.Cross(normal, look);
        Vector3 levelToGroundLook = Vector3.Cross(intermediate, normal);
        return levelToGroundLook;
    }
    float lastdmg = 0;

    Vector3 NormalFromRayCast()
    {
        Vector3 startpoint = transform.position + transform.up * 1.15f + transform.forward*2.8f; // transform position is basically on the ground so go up a lil
        Vector3 dir = (-transform.up + transform.forward * 0.1f).normalized;
        RaycastHit hit;
        if(!Physics.Raycast(startpoint, dir*3, out hit)) UnityEngine.Debug.Log("NOHIT");
        //string f = hit.rigidbody.gameObject.name;
        // if (hit.rigidbody.gameObject.name == "player") { UnityEngine.Debug.Log("fr");  }
        if (hit.collider == null) return (transform.position-daddy.transform.position);
        return hit.collider.gameObject.name=="player" ? lastNormal : hit.normal;

    }

    float getRayCastDistFromGround()
    {
        float upstart = 1.85f;
        Vector3 startpoint = transform.position + transform.up * upstart; // transform position is basically on the ground so go up a lil
        Vector3 dir = (-transform.up + transform.forward * 0.1f).normalized;
        RaycastHit hit;
        if (!Physics.Raycast(startpoint, dir * 10, out hit)) { UnityEngine.Debug.Log("NOHIT"); return 10; };
        return hit.distance-upstart;
    }

    private GameObject closestPlanet()
    {
        float closest = 0;
        GameObject winner = null ;
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Planet"))
        {
            float dist = (p.transform.position - transform.position).magnitude;
            if (dist < closest || closest == 0)
            {
                closest = dist;
                winner = p;
            }
        }
       //  UnityEngine.Debug.Log("winner is " + winner.name); 
        if (winner == null) UnityEngine.Debug.Log("BADTHINGSAREHAPPENING");
        return winner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "playerAttack")
        {
            if (Time.time - lastdmg < .15f) return;
            lastdmg = Time.time;
            Destroy(other.gameObject, 0);
            hp--;
            if(hp<5)
                GetComponent<ninja>().canDodge = true;
            // UnityEngine.Debug.Log("IMHIT2koirjasfjfdjh");
            if (hp > 0 &&!running)
            {
                justhit = true;
                animator.SetBool("Hit", true);
            }
            else if(hp<=0)
            {
                dead = true;
                animator.SetBool("Dead", true);
            }
        }

    }




}
