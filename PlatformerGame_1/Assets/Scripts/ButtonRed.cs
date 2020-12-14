using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRed : MonoBehaviour
{
    public GameObject[] blocks;
    public Sprite turnOn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Weight"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = turnOn;
            gameObject.GetComponent<Collider2D>().enabled = false;

            foreach(GameObject block in blocks)
            {
                Destroy(block);
            }
        }
    }
}
