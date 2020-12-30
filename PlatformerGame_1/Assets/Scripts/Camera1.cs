using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera1 : MonoBehaviour
{
    public Transform playerTr;
    float speed = 3f;
    
    void Start()
    {
        transform.position = new Vector3(playerTr.position.x, playerTr.position.y, transform.position.z);
    }

    void Update()
    {
        Vector3 position = playerTr.position;
        position.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
