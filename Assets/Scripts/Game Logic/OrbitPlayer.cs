using UnityEngine;
using System.Collections;

public class OrbitPlayer : MonoBehaviour
{
    public GameObject target;
    private Vector3 targetPosition;
    public float rotateSpeed;
    public float minOrbitDistance = 6;
    public float moveTowardSpeed;
    private Vector3 rotateAgainstDirection;
    void Start()
    {
        rotateAgainstDirection = Vector3.up;
    }

    void Update()
    {
        target = GameObject.Find("player");
        targetPosition = target.transform.position;
        Vector3 playerDirection = target.transform.forward;

        //if (Time.time > nextRotationChangeTime)
        //{
        //    nextRotationChangeTime += Random.Range(3,7);
        //    // Switch rotation direction
        //    if (rotateAgainstDirection == Vector3.up)
        //        rotateAgainstDirection = Vector3.down;
        //    else
        //        rotateAgainstDirection = Vector3.up;
        //}

        Vector3 moveToPosition = target.transform.position + playerDirection * minOrbitDistance;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveToPosition, moveTowardSpeed * Time.deltaTime);
        gameObject.transform.RotateAround(targetPosition, rotateAgainstDirection, rotateSpeed * Time.deltaTime);
    }
}