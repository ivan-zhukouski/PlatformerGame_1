using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 4f;

    void Start()
    {
        StartCoroutine(Disable());
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
           Destroy(gameObject);
    }
}
