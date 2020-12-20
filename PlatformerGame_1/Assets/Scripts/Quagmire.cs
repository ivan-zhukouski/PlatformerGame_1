using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quagmire : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().mass *= 100f;
            collision.gameObject.GetComponent<Player>().moveSpeed *= 0.25f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().mass *= 0.01f;
            collision.gameObject.GetComponent<Player>().moveSpeed *= 4;
        }
    }
}
