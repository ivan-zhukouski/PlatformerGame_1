using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float bouncyForce;
    public bool isPlayerHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isPlayerHit)
        {
            collision.gameObject.GetComponent<PlayerMove>().RecountHP(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * bouncyForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator Death()
    {
        gameObject.GetComponent<Animator>().SetBool("dead", true);
        isPlayerHit = true;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject, 2f);
    }

    public void StartDeath()
    {
        StartCoroutine(Death());
    }
}
