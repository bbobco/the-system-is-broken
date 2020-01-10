using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class AnimateVideoPlaybackSpeed : MonoBehaviour
{


    VideoPlayer video;


    public float playBackSpeed = 1F;

    //public float randomMin = 0.25F;
    //public float randomMax = 0.5F;

    public float increment = 0.05F;
    public float time = 0F;


    // Start is called before the first frame update
    void Start()
    {
        video = gameObject.GetComponent(typeof(VideoPlayer)) as VideoPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        //if (playBackSpeed > 1 || playBackSpeed < 0.1)
        //    increment *= -1F;
        //playBackSpeed += increment;
        // playBackSpeed = Random.Range(randomMin, randomMax);

        time += increment;
        playBackSpeed = Mathf.PerlinNoise(time, 0);
        video.playbackSpeed = playBackSpeed;
   
    }
}
