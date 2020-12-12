using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundPatrol: MonoBehaviour
{
    public Transform groundDetect;
    float speed = 1f;
    public bool moveLeft = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector3.down, 1f);

        if(!groundInfo.collider)
        {
            if(moveLeft)
            {
                transform.eulerAngles = new Vector3(0, 180f, 0);
                moveLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                moveLeft = true;
            }
        }
    }
}
