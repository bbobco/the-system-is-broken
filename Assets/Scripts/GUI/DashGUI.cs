using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashGUI : MonoBehaviour
{   
    
    public Vector2 BarSize;

    // 2d rendering 4 dummies
    private CanvasRenderer cr;
    private Canvas canvas;
    private CanvasScaler durkScale;
    private Image bar;
    private FirstPersonController playerController;
    private RectTransform screen;
    private GameObject UIobj;

    // dash cd timer
    private float timeInDash;

    // Start is called before the first frame update
    void Start()
    {   
        playerController = GetComponent<FirstPersonController>();
        UIobj = new GameObject("UI");
        UIobj.transform.parent = playerController.gameObject.transform;

        canvas = UIobj.AddComponent<Canvas>();
        cr = UIobj.AddComponent<CanvasRenderer>();
        durkScale = UIobj.AddComponent<CanvasScaler>();
        bar = UIobj.AddComponent<Image>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    void Update() {
        if ( playerController.getDashCD() ) {
            timeInDash += Time.deltaTime;

            float totalCD = playerController.DashCD;
            setPercent( ( totalCD - timeInDash) / totalCD );
        } else {
            timeInDash = 0;
            setPercent(0);
        }
        
    }

    private void setPercent( float percent ) {
        cr.EnableRectClipping( new Rect( 
            -Screen.width/2, 
            -Screen.height/2, 
            BarSize.x * percent, 
            BarSize.y));
    }

}