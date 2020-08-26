using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private float angle;
    [Header("Angle")]
    public float AngleIterator = 1f;
    public bool xAxis = false;
    public bool yAxis = false;
    public bool zAxis = false;
    // Start is called before the first frame update
    private Vector3 Rotation;
    void Start()
    {
        Rotation = new Vector3(0f, 0f, 0f);
        if (xAxis)
            Rotation.x = AngleIterator;
        if (yAxis)
            Rotation.y = AngleIterator;
        if (zAxis)
            Rotation.z = AngleIterator;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Rotation);
    }

}
