using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airPatrol : MonoBehaviour
{
    public Transform[] points;
    float speed = 2f;
    bool isGoing = true;
    float waitTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(points[0].position.x, points[0].position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(isGoing)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[0].position, speed * Time.deltaTime);
        }
       
        if(transform.position == points[0].position)
        {
            Transform t = points[0];
            points[0] = points[1];
            points[1] = t;
            isGoing = false;
            StartCoroutine(Wait());

        }
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(waitTime);
            isGoing = true;
        }
    }
}
