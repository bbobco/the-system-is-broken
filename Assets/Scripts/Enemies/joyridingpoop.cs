using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class joyridingpoop : MonoBehaviour
{
    public GameObject[] planets;
    public GameObject target;
    float timeOfLastAction = -533f;
    public float timeBetweenActions = 1.75f;
    Vector3 homingrandom = Vector3.zero;
    float timeborn = 0;
    public int groupActivity = 0;
    bool isleader = false;


    public bool manualControl = false;
    public bool slowboy = false;
    public bool homing;
    public float overheadDistance = 30;
    public bool teamdawg = false;
    public bool customtask = false;

    /// <summary>
    /// /********* need to remove all 420 of these booleans and just make different extensions of this class for different purposes
    /// </summary>
    void Awake()
    {
       // target = chooseRandomObject(GameObject.FindGameObjectsWithTag("asteroid"));
        timeborn = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public float speedmultiplier = 1;
    const int FOLLOW_LEADER = 420;
    const int SQUAD_UP = 3;
    const int ASTEROID_CHASING = 1;
    const int PLAYER_APPROACH = 2;
    public int myAction = 0;
    // Update is called once per frame
    Quaternion quaturdion = new Quaternion();
    Vector3 newdir;
    float speed = 0;
    float lastCalculatedDir = 0;
    float timeOfLastShot = 0;
    float shootTimer = 0.38f*1.5f;
    Vector3 dir;
    joyridingpoop leader = null;
    float lasthomingRandom = 0;
    bool newTargetNextTime = false;
    void FixedUpdate()
    {
        
        // UnityEngine.Debug.Log("dsgdgsgdssgdgds " + GameObject.FindGameObjectsWithTag("asteroid").Length);
        float force = 2700;
        bool timeForNewTarget = false;
        if (target == null) timeForNewTarget = true;
        if (Time.time > timeOfLastAction + timeBetweenActions)
        {
            timeForNewTarget = true;
            timeOfLastAction = Time.time;
        }
        bool farFromEverything = false;
        int numpossibilities = 0;

        if (target == null) timeForNewTarget = true;

        if ( target != null&&(transform.position - target.transform.position).magnitude < target.transform.localScale.z / 2 + 55 && target.tag.Equals("asteroid") &&!manualControl )
        {
            timeForNewTarget = true;
            UnityEngine.Debug.Log("got to target asteroid");

        }

        if (timeForNewTarget || newTargetNextTime)
        {

            newTargetNextTime = false;
            GameObject[] squad = GameObject.FindGameObjectsWithTag("SQUAD");
            
            
            float whatToDo = UnityEngine.Random.Range(0.5f, 3f);
            if (myAction==FOLLOW_LEADER) whatToDo = -1;
            if (homing) whatToDo = 7.10f/4.20f;
            if (slowboy) whatToDo = 4.20f - .69f * 3;
            if (manualControl) whatToDo = 4.20f - .69f * 3;
            if (whatToDo < 1.5f && whatToDo>0)
            {
                myAction = ASTEROID_CHASING;
                target = chooseRandomObject(GameObject.FindGameObjectsWithTag("asteroid"));
                if (homing) target = GameObject.Find("player");
                

            }
            if (whatToDo >= 1.5f && whatToDo<3)
            {
                myAction = PLAYER_APPROACH;
                speed = 0.6f*speedmultiplier;
             //   UnityEngine.Debug.Log("doin the number 2 do");
                 if(!manualControl)
                    target = GameObject.Find("player");
                if (teamdawg)
                {
                    GameObject[] targets = GameObject.FindGameObjectsWithTag("enemy");
              //      UnityEngine.Debug.Log("targets length is " + targets.Length);

                    for (int x = 0; x < targets.Length; x++)
                    {
                        int r = UnityEngine.Random.Range(0, targets.Length - 1);
                        if (targets[r] == null) continue;
                        target = targets[r];
                        if (target == null) continue;
                        if (target.GetComponent<GravityBody>() != null && target.GetComponent<GravityBody>().winner != null)
                        {
                            if ((transform.position - target.GetComponent<GravityBody>().winner.transform.position).magnitude < (transform.position - target.transform.position).magnitude) continue;
                            if ((target.transform.position - target.GetComponent<GravityBody>().winner.transform.position).magnitude < target.GetComponent<GravityBody>().winner.transform.localScale.x / 2 + 24)
                                break;
                        }
                    }
                 //   UnityEngine.Debug.Log("target loc is " + target.transform.position);
                }

            }
            if(whatToDo>=3)
            {
                
                myAction = SQUAD_UP;
                speed = 0.95f*speedmultiplier;

                target = chooseRandomObject(GameObject.FindGameObjectsWithTag("SQUAD"));
                if(target.gameObject == this.gameObject || isleader)
                {
                    myAction = ASTEROID_CHASING;
                }
                else
                {
                }
            }
        }

        if (myAction == ASTEROID_CHASING)
        {
            speed = .65f*speedmultiplier;
            goToLocationWithoutCollision(target.transform.position);
        }
        if (myAction == PLAYER_APPROACH)
        {
            speed = .20f*speedmultiplier;
            //   UnityEngine.Debug.Log("gravbody winner pos is " + target.GetComponent<GravityBody>().winner.transform.position);
            Vector3 playersNormal = new Vector3(0,0,0);
            if (target == null) UnityEngine.Debug.Log("target null brody");
            if(target!=null && target.GetComponent<GravityBody>()!=null && target.GetComponent<GravityBody>().winner!=null)
             playersNormal = ( target.transform.position-target.GetComponent<GravityBody>().winner.transform.position).normalized;
            
            Vector3 overheadloc = target.transform.position + playersNormal * overheadDistance;
            overheadloc += homingrandom;

            if((slowboy || homing) &&UnityEngine.Random.Range(0,1f)>.55f && (transform.position- overheadloc).magnitude> 10f && Time.time-lasthomingRandom>.4f)
            {

                //UnityEngine.Debug.Log("funky town");
                homingrandom = randomvec() * UnityEngine.Random.Range(3f, 22f)*speed;
                lasthomingRandom = Time.time;
            }
            if (Physics.Raycast(transform.position + transform.forward * 22, transform.forward, 20))
                newTargetNextTime = true;
            //desired behavior is to fly overhead, not stop moving when directly over player
            if ((overheadloc - transform.position).magnitude < overheadDistance*1.35f+18*0 && !homing && !manualControl)
            {
                newTargetNextTime = true;
                UnityEngine.Debug.Log("APPARENTLY IM CLOSE");
              //*****************************************
                overheadloc += transform.forward * 4 + playersNormal*8;
            }
            /*
            else if ( (overheadloc - transform.position).magnitude < overheadDistance+20 && slowboy)
            {
                speedmultiplier = 0;
                GameObject.Find("object__0").GetComponent<bigboy>().deathrayActive = true;
            }
            */
            goToLocationWithoutCollision(overheadloc);//new Vector3(0, 0, 5));


            if (Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized) > 0.90f && Time.time - timeOfLastShot > shootTimer)
            {
               // UnityEngine.Debug.Log("shoooooting");
                timeOfLastShot = Time.time;
                if(!manualControl)
                    shoot();
            }
            

            //     GameObject player = GameObject.Find("player")
            //    if (Vector3.DOT(dirToObject(player).normalized,  ) ) 
            // transform.forward = GameObject.Find("player").transform.right;
        }
        //not being used rn 
        /*
        if(myAction == SQUAD_UP)
        {
            speed = 2.75f;
            goToLocationWithoutCollision(target.transform.position);
            if ((target.transform.position - transform.position).magnitude < 25)
            {
                if (target.GetComponent<joyridingpoop>().isleader == false)
                {
                    if (target.GetComponent<joyridingpoop>().groupActivity == 0)
                    {
                        isleader = true;
                        target.GetComponent<joyridingpoop>().groupActivity = 1;
                        target.GetComponent<joyridingpoop>().leader = GetComponent<joyridingpoop>();
                        target.GetComponent<joyridingpoop>().myAction = FOLLOW_LEADER;
                        myAction = ASTEROID_CHASING;
                    }
                    else
                    {
                        leader = target.GetComponent<joyridingpoop>().leader;
                        myAction = FOLLOW_LEADER;
                    }
                }
                else if (target.GetComponent<joyridingpoop>().isleader == true)
                {
                    leader = target.GetComponent<joyridingpoop>();
                    myAction = FOLLOW_LEADER;
                }
            }
        }
        if(myAction == FOLLOW_LEADER)
        {
            //UnityEngine.Debug.Log("FOLLOWING LEADER ");
            target = leader.target;
           // dir = leader.dir;
            UnityEngine.Debug.Log("dir is "+dir);
            //TERRIBLE TEMPORARY WORKAROUND
          //  Vector3 empty = Vector3.zero;
            goToLocationWithoutCollision(target.transform.position+new Vector3(15, 0, 0));
        }
        */
        //GetComponent<Rigidbody>().AddForce(force*Time.fixedDeltaTime*dir);


    }

    public GameObject projectile;
    float shotspeed = 125*1;
    void shoot()
    {

        if (!teamdawg)
        {
            float randomnessToShotDirection = 0.16f;
            Vector3 shotdir = ((target.transform.position - transform.position).normalized + randomvec() * randomnessToShotDirection).normalized;
            //Vector3 shotdir = transform.forward;
            //UnityEngine.Debug.Log("shotdir " + shotdir.magnitude );
            var p = Instantiate(projectile, (transform.position + shotdir * 15), Quaternion.identity);
            Destroy(p, 8/2);

            p.transform.Rotate(90, 0, 0, Space.Self);
            p.GetComponent<Rigidbody>().AddForce(shotdir * shotspeed, ForceMode.VelocityChange);
           

        }

        //predictive shot
        if(teamdawg)
        {
            
            float dist = (target.transform.position - transform.position).magnitude;


            Vector3 sdir = (target.transform.position - transform.position).normalized;
            
            Vector3 gunLocation = sdir * 14;
            //if theres something else in front of the target change targets
            /*
            if (Physics.Raycast(transform.position + gunLocation, sdir, dist - 45) && dist<150)
            {
                newTargetNextTime = true;
                return;
            }
            */
            //      Vector3 sdir = (target.transform.position - transform.position-gunLocation).normalized;
            float travelTime = (target.transform.position - transform.position-gunLocation).magnitude / shotspeed*2; ///*2 made it more accurate idk why
            Vector3 adjustedTarget = target.transform.position + target.GetComponent<Rigidbody>().velocity / travelTime;
            Vector3 adjusteddir = (adjustedTarget - transform.position-gunLocation).normalized;

            var p = Instantiate(projectile, (transform.position + adjusteddir * 14), Quaternion.identity);
            p.GetComponent<Rigidbody>().AddForce(adjusteddir * shotspeed, ForceMode.VelocityChange); 
         //   UnityEngine.Debug.Log("actual shot velocity is " + p.GetComponent<Rigidbody>().velocity.magnitude);
            Destroy(p, 8);
        }



       // p.GetComponent<Rigidbody>().velocity = shotdir * shotspeed;
    }
    bool right, left, up, down = false;

    float dodgetime = 0;
    Vector3 getClosestDirectionWithNoCollision(Vector3 desiredDirection)
    {
        desiredDirection = desiredDirection.normalized;
        float distanceChecked =35;

        float distancetoFront = transform.localScale.z / 2 + 1; 
        if (manualControl) //just doing this so the miner doesn't dodge the aseteroid its going for
            distancetoFront = 14+target.transform.localScale.x;
        bool good2go = !Physics.Raycast(transform.position+transform.forward* distancetoFront, desiredDirection, distanceChecked);
        if (good2go && Time.time - dodgetime> .4f) { right = false;up = false;left = false;down = false; dodgetime = 99999f; }
        

        Vector3 newdir = desiredDirection;



            //could loop this to be more accurate with bigger turns if it doesnt find a direction with no collision
            float turnStrength = .15f;
        //with addition of up/down/left/right being reset every time there is no collision, we can ensure that we will keep dodging each individual object in the same direction
        //without this there would be problem of try to dodge object right, next iteration try to dodge same object to left etc...
        while (!good2go)
        {
            if (!good2go&&!left&&!up&&!down)
            {
                newdir = (desiredDirection + transform.right.normalized * turnStrength).normalized;
                good2go = !Physics.Raycast(transform.position + transform.forward * distancetoFront, newdir, distanceChecked);
                if (good2go) right = true;
            }
            
            if (!good2go && !right && !up && !down)
            {
                newdir = (desiredDirection - transform.right.normalized * turnStrength).normalized;
                good2go = !Physics.Raycast(transform.position + transform.forward * distancetoFront, newdir, distanceChecked);
                if (good2go) left = true;
            }
            if (!good2go && !left && !down && !right)
            {
                newdir = (desiredDirection + transform.up * turnStrength).normalized;
                good2go = !Physics.Raycast(transform.position + transform.forward * distancetoFront, newdir, distanceChecked);
                if (good2go) up = true;
            }
            if (!good2go && !left && !up && !right)
            {
                newdir = (desiredDirection - transform.up * turnStrength).normalized;
                good2go = !Physics.Raycast(transform.position + transform.forward * distancetoFront, newdir, distanceChecked);
                if (good2go) down = true;
            }
       //     if (!good2go) UnityEngine.Debug.Log("over 1 turn");
            turnStrength += .2f;
            if (turnStrength > 4.15f) break; //rip
        }
        if (good2go && Time.time-dodgetime>.4f) { dodgetime = Time.time; }
        if (!good2go)
            UnityEngine.Debug.Log("CANT GO NOWHERE");
        
        return newdir;
    }
    Quaternion q = new Quaternion();
    Quaternion initial = new Quaternion();
    public float minTimebetweenTurns = 0.25f;
    //should remove follow leader behavior from this -- shouldnt be in this function
    public float s = 0;
    void goToLocationWithoutCollision(Vector3 targetpos)
    {

        float dist;
        bool closeEnough=false;
        if (targetpos != Vector3.zero)
        {
            //getClosestDirectionWithNoCollision(targetpos-transform.position);
            if (Time.time - lastCalculatedDir > minTimebetweenTurns)
            {
                dir = targetpos - transform.position;

                dir = dir.normalized;
                dir = getClosestDirectionWithNoCollision(dir);
                s = 0;
                lastCalculatedDir = Time.time;
            }
            dist = (targetpos - transform.position).magnitude;
            closeEnough = dist < (target.transform.localScale.x / 2 + 10);
            if (manualControl)
            {
                closeEnough = dist < (transform.localScale.z / 2 + target.transform.localScale.x / 2 + 6);
                if (overheadDistance>25)
                    closeEnough = dist < (transform.localScale.z / 2  + 6);

            }
        }
        
        if (s==0 )
        {
            initial = transform.rotation;
            q = new Quaternion();
            q.SetFromToRotation(transform.forward, dir);
            //        if (ss < .06f) { qqq = q*transform.rotation;  qq = q; }
            q = q * transform.rotation;
           // s = 1;
       }
        
   



        //dir = dir.normalized;

        if (!closeEnough)
        {
            if (homing)
                transform.position += new Vector3(Mathf.Sin(Time.time*4 + timeborn)/4f, Mathf.Sin(Time.time * 4 + timeborn)/4f, Mathf.Cos(Time.time*4 + timeborn)/4f );
            //transform.position += dir * speed;
            float dot = Vector3.Dot(dir, (target.transform.position - transform.position).normalized);
            //UnityEngine.Debug.Log("dot " + dot);
            if (dot < .60f ) speed /= 1.6f;

            //transform.rotation = q;
            //   float nonjerkModifier = (Time.time - timeOfLastAction) / timeBetweenActions/2*0+1;
            //   float speedjerkModifier =nonjerkModifier;

            float speedmod = 1;//(transform.position - target.transform.position).magnitude / 120;

            //  ss = (Time.time - timeOfLastAction) / timeBetweenActions;
            // UnityEngine.Debug.Log(ss);
            // transform.rotation = Quaternion.Slerp(qqq, qq, ss);

            //SLERPPPPPPPPPPPPPPPPPP
            // s += .08f*speedmultiplier;
            // s += .01f;
            s+=1f/minTimebetweenTurns/100f;
            if (s > 1) s = 1;
             transform.rotation = Quaternion.Slerp(initial, q, Time.fixedDeltaTime*0+  s ); //definitely was using slerp wrong  fix later

            Vector3 dur = transform.rotation * Vector3.forward;

            transform.position += transform.forward * speed * speedmod; // why not working
           //transform.position += dur* speed * speedjerkModifier; // why not working


        }

       
    }
 //   float ss = 0;
   // Quaternion qq = new Quaternion();
  //  Quaternion qqq = new Quaternion();
    float distFromObject(GameObject o)
    {
        return (o.transform.position - transform.position).magnitude;
    }
    Vector3 dirToObject(GameObject o)
    {
        return (o.transform.position - transform.position);
    }
    public GameObject chooseRandomObject(GameObject[] o)
    {
        return o[UnityEngine.Random.Range(0, o.Length - 1)];
    }

    public static Vector3 randomvec()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
    
}
