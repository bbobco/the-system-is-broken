using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //FirstPersonController isPlayer = other.GetComponent<FirstPersonController>();
        if (other.tag == "enemyProjectile")
        {
            HealthBar.SetHealthBarValue(HealthBar.GetHealthBarValue() - .1f);
            Destroy(other.gameObject, 1);
        }
        if (other.tag == "healthUp")
        {
            HealthBar.SetHealthBarValue(HealthBar.GetHealthBarValue() + .33f);
            Destroy(other.gameObject); //Not Destroying it -- bug
        }
    }
}


