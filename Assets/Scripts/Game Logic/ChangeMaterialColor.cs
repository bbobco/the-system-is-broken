using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{

    private Material[] gunMaterials;
    private Material ropeMaterial;
    // Start is called before the first frame update
    void Start()
    {
        gunMaterials = GameObject.Find("ChargeGunModel").GetComponent<Renderer>().materials;
    }
    
    void FixedUpdate()
    {
        float hue = (1f + Mathf.Sin(Time.time)) * 0.5f;
        Color color = Color.HSVToRGB(hue , 0.5f, 0.5f);
        for ( int i = 0; i < gunMaterials.Length; i++)
        {
            gunMaterials[i].SetColor("_Color", color);
        }
        // gameObject.GetComponent<LineRenderer>().material.SetColor("_Color", color);
        gameObject.GetComponent<Grapple>().setColor(color);
    }
}
