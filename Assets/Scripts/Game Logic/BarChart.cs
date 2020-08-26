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
        prism = Instantiate(Resources.Load("Meshes/BarChartMesh", typeof(GameObject))) as GameObject;
        prism.AddComponent<OrbitPlayer>();
        GameObject player = GameObject.Find("player");
        prism.GetComponent<OrbitPlayer>().target = player;
        prism.GetComponent<OrbitPlayer>().moveTowardSpeed = 4;
        prism.GetComponent<OrbitPlayer>().rotateSpeed = 21;
        prism.transform.localPosition = new Vector3(0, -1.2F, 0);
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
            if (growthState == 1 && prism.transform.localScale.y > 15)
            {
                growthState = -1;
                remainingFrames = 150;
            }
            // If the bar chart shrinks too small, pause shrinkage before growing
            else if (growthState == -1 && prism.transform.localScale.y <= 5)
            {
                growthState = 2;
                remainingFrames = 150;
            }
            if (growthState == 1)
            {
                prism.transform.localScale += new Vector3(.02F, .08F, .02F);
                prism.transform.position += new Vector3(0F, .04F, .0F);
            }
            else if (growthState == -1)
            {
                prism.transform.localScale -= new Vector3(.02F, .08F, .02F);
                prism.transform.position -= new Vector3(0F, .04F, .0F);
            }
            remainingFrames--;
        }
        else
        {
            // If the bar chart prism is growing too big, pause growth before shrinking
            if (growthState == 1 && prism.transform.localScale.y > 15)
            {
                growthState = -1;
                remainingFrames = 180;
            }
            // If the bar chart shrinks too small, pause shrinkage before growing
            else if (growthState == -1 && prism.transform.localScale.x <= 5)
            {
                growthState = 2;
                remainingFrames = 120;
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
                remainingFrames = 60;
            }
        }

    }
}


