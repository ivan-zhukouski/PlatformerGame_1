using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public Transform bulletStart;
    public GameObject bullet;
    public float fireSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToInstantiate());
    }

    // Update is called once per frame
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
