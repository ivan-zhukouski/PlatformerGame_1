using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beetle : MonoBehaviour
{
    public Transform point;
    bool isHidden = true;
    bool isWaiting = false;
    float speed = 3f;
    public float waiting;
    // Start is called before the first frame update
    void Start()
    {
        point.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isWaiting)
            transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);

        if(transform.position == point.position)
        {
            if(isHidden)
            {
                isHidden = false;
                point.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
            }
            else
            {
                isHidden = true;
                point.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            }
            isWaiting = true;
            StartCoroutine(Waiting());
        }
        
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waiting);
        isWaiting = false;
    }
}
