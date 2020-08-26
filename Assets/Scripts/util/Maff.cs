using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Maff
{
    public static Vector3 FastestPathOnSphere(Vector3 look, Vector3 normal)
    {
        Vector3 intermediate = Vector3.Cross(normal, look);
        Vector3 levelToGroundLook = Vector3.Cross(intermediate, normal);
        return levelToGroundLook;
    }
}
