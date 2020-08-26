using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public GameObject effect;
  //  public AudioClip ouchie;
    float lasthit = 0;
 //   public AudioSource a;
    // Start is called before the first frame update
    void Start()
    {
  //      a = GameObject.Find("player").GetComponent<AudioSource>();
   //     a.clip = ouchie;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //FirstPersonController isPlayer = other.GetComponent<FirstPersonController>();
        if (other.tag == "enemy" && Time.time-.33f>lasthit)
        {
            lasthit = Time.time;
            HealthBar.SetHealthBarValue(HealthBar.GetHealthBarValue() - .15f);

            AudioSource a = GameObject.Find("player").GetComponent<AudioSource>();

            for (int x = 0; x < 4; x++)
            {
                var p = Instantiate(effect, transform.position + transform.forward * 1.4f, Quaternion.identity);
                p.transform.localScale *= 1f;
                Destroy(p, 2.5f);
            }
            
            // a.Play();
        }
        if (other.tag == "enemyProjectile")
        {
            HealthBar.SetHealthBarValue(HealthBar.GetHealthBarValue() - .1f);
            Destroy(other.gameObject, 1);

            AudioSource a = GameObject.Find("player").GetComponent<AudioSource>();
          //  a.Play();
        }
        if (other.tag == "healthUp")
        {
            HealthBar.SetHealthBarValue(HealthBar.GetHealthBarValue() + .33f);
            Destroy(other.gameObject); //Not Destroying it -- bug

            AudioSource a = GameObject.Find("player").GetComponent<AudioSource>();
          //  a.Play();
        }
        if(HealthBar.GetHealthBarValue() <=0)
        {
            GameObject.Find("player").GetComponent<FirstPersonController>().dead = true;
            GameObject.Find("player").GetComponent<FirstPersonController>().timeOfDeath= Time.time;
        }
    }
}


