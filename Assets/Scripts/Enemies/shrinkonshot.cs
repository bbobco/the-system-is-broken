using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class shrinkonshot : MonoBehaviour
{
    public int hp = 1;
    public GameObject effect;
    public GameObject hiteffect;
    bool shrinking = false;
    public int sizeofdeath = 0;
    float sizeBeforeShrink = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float shrinkIncrement = .08f;
        if(shrinking)
        {
            Vector3 shrinkAmount = new Vector3(shrinkIncrement,shrinkIncrement,shrinkIncrement);
            transform.localScale -= shrinkAmount;
  //          if (transform.localScale.x + 2.5 < sizeBeforeShrink)
 //               shrinking = false;
           if (transform.localScale.magnitude < .1f)
                Destroy(transform.root.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
      //  UnityEngine.Debug.Log("AYWRRRRRR");
        //FirstPersonController isPlayer = other.GetComponent<FirstPersonController>();
        if (other.tag == "playerAttack" || other.tag == "enemybullet")
        {
            if (gameObject.tag == "SQUAD" && other.tag == "enemybullet")
                return;
            
            if(hp>0)
            {
                sizeofdeath++;
                hp--;
                var p = Instantiate(hiteffect, other.transform.position, Quaternion.identity);
                p.transform.localScale *= 1;
                Destroy(p, 2.5f);
            }
         //   UnityEngine.Debug.Log("hp is " + hp);
            if (hp <= 0)
            {
                //   UnityEngine.Debug.Log("im ded dawg");
                death();
            }

            
            /*
            sizeBeforeShrink = transform.localScale.x;
            shrinking = true;
            Destroy(other.gameObject, 1);  
            */
        }
        
    }
    public void death()
    {
        for (int x = 0; x < 1 * sizeofdeath; x++)
        {
            //       GameObject scaledEffect = effect;
            //      scaledEffect.transform.localScale *= (transform.localScale.x / 9);
            var p = Instantiate(effect, transform.position + joyridingpoop.randomvec() * UnityEngine.Random.Range(1f, transform.localScale.x / 2), Quaternion.identity);
            p.transform.localScale *= transform.localScale.x / 7 * UnityEngine.Random.Range(1, sizeofdeath / 2);
            Destroy(p, 1f);
            if (x > 22)
            {
                p = Instantiate(effect, transform.position + joyridingpoop.randomvec() * UnityEngine.Random.Range(1f, transform.localScale.x / 2), Quaternion.identity);
                p.transform.localScale *= 200;
            }
        }
        //          p.transform.localScale *= .04f;
        Destroy(this.gameObject, 0.0f);

        moneydisplay.money++;
    }
}
