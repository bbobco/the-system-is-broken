using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChart : MonoBehaviour
{
    GameObject prism;
    public int remainingFrames = 120;
    public enum growthStateChoices
    {
        changeless = 0,
        increasing = 1,
        pausedIncreasing = 2,
        decreasing = -1,
        pausedDecreasing = -2
    }
    public int growthState = 2;

    // Start is called before the first frame update
    void Start()
    {
        prism = GameObject.CreatePrimitive(PrimitiveType.Cube);
        prism.transform.parent = gameObject.transform;
        prism.transform.localPosition = new Vector3(0, 0.5f, 0);
        prism.transform.localScale = new Vector3(1F, 4F, 1F);
        prism.tag = "BarChart";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (remainingFrames > 0)
        {
            // If the bar chart prism is growing too big, pause growth before shrinking
            if (growthState == 1 && prism.transform.localScale.y > 24)
            {
                growthState = -1;
                remainingFrames = 150;
            }
            // If the bar chart shrinks too small, pause shrinkage before growing
            else if (growthState == -1 && prism.transform.localScale.x <= 1)
            {
                growthState = 2;
                remainingFrames = 150;
            }
            if (growthState == 1)
            {
                prism.transform.localScale += new Vector3(.025F, .1F, .025F);
            }
            else if (growthState == -1)
            {
                prism.transform.localScale -= new Vector3(.025F, .1F, .025F);
            }
            remainingFrames--;
        }
        else
        {
            // If the bar chart prism is growing too big, pause growth before shrinking
            if (growthState == 1 && prism.transform.localScale.y > 24)
            {
                growthState = -1;
                remainingFrames = 150;
            }
            // If the bar chart shrinks too small, pause shrinkage before growing
            else if (growthState == -1 && prism.transform.localScale.x <= 1)
            {
                growthState = 2;
                remainingFrames = 150;
            }
            else
            {
                if (growthState == 1 || growthState == -2)
                {
                    growthState++;
                }
                else if (growthState == 2 || growthState == -1)
                {
                    growthState--;
                }
                remainingFrames = 90;
            }
        }
    }
}


