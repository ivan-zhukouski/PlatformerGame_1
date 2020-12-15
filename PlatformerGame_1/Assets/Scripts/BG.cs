using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    public GameObject cam;
    float bgLength, startpos;
    public float parallaxEffect;

    void Start()
    {
        startpos = transform.position.x;
        bgLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);

        float distance = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if(temp > startpos + bgLength)
        {
            startpos += bgLength;
        }else if(temp < startpos - bgLength)
        {
            startpos -= bgLength;
        }
    }
}
