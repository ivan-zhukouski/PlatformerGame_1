using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airPatrol : MonoBehaviour
{
    public Transform[] points;
    public float speed = 2f;
    bool isGoing = true;
    public float waitTime;

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
            
            if (transform.rotation.y == 0)
                transform.eulerAngles = new Vector3(0, 180, 0);
            else
                transform.eulerAngles = new Vector3(0, 0, 0);
            isGoing = true;
        }
    }
}
