using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public Transform bulletStart;
    public GameObject bullet;
    public float fireSpeed;

    void Start()
    {
        StartCoroutine(WaitToInstantiate());
    }

    void Update()
    {
    }

    IEnumerator WaitToInstantiate()
    {
        yield return new WaitForSeconds(fireSpeed);
        Instantiate(bullet, bulletStart.position, transform.rotation);
        StartCoroutine(WaitToInstantiate());
    }
}
