using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAirPatrol : MonoBehaviour
{
    public Transform[] points;
    public float speed;
    public float waitTime;
    bool isGoing = true;
    int i = 1;

    void Start()
    {
        transform.position = new Vector3(points[0].position.x, points[0].position.y, transform.position.z);
    }

    void Update()
    {
        if(isGoing)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
        if(transform.position == points[i].position)
        {
            if(i< points.Length - 1)
            {
                i++;
            }
            else
            {
                i = 0;
            }
            isGoing = false;
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        isGoing = true;
    }
}
