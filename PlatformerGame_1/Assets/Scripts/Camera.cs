using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform targetTransform;
    float speed = 3f;
    
    void Start()
    {
        transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = targetTransform.position;
        position.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
