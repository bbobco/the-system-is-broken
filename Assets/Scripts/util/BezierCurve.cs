using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
{
    private static Vector2[] points = { new Vector2(0.0f,0.0f),
                            new Vector2(0.5f,0.0f),
                            new Vector2(0.5f,1.0f),
                            new Vector2(1.0f,1.0f) };
    public static float Curve(float x) {
        float t = (1.0f - x);
        float y = t*t*t * points[0].y;
        y += 3 * t*t * x * points[1].y;
        y += 3 * t * x*x * points[2].y;
        y += x*x*x * points[3].y;

        return y;
    }
}
