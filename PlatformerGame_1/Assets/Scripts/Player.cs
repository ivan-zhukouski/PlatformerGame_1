using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    Rigidbody2D playerRB;
    public float moveSpeed;
    float jumpForce = 14f;
    float horizontalMove;
    bool isGrounded;
    public Transform groundCheck;
    Animator animator;
    int maxHP = 3;
    int currentHP;
    SpriteRenderer spriteRenderer;
    public Mein mein;
    float waitTime = 0.02f;
    Color defaultColor;
    bool gotKey = false;
    bool canTP = true;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        CheckGround();
        
        if (horizontalMove == 0 && isGrounded)
        {
            animator.SetInteger("State", 0);
        } else
        {
            if (isGrounded)
            {
                animator.SetInteger("State", 1);
            }
        }
        if(!isGrounded || isGrounded)
        {
            FlipPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(horizontalMove * moveSpeed, playerRB.velocity.y);
        
    }

    void FlipPlayer()
    {
        if(horizontalMove > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if(horizontalMove < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }
        
    }

    void Jump()
    {
        playerRB.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        
        isGrounded = colliders.Length > 1;
        if(!isGrounded)
        {
            animator.SetInteger("State", 2);
        }
    }

    public void RecountHP(int deltaHP)
    {
        currentHP += deltaHP;
        if(deltaHP < 0)
        {
            StartCoroutine(OnHit());
        }
        if(currentHP <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled =  false;
            Invoke("Lose", 1.3f);
        }
    }

    IEnumerator OnHit()
    {
        spriteRenderer.color = new Color(1f, 0.3f, 0.3f);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = defaultColor;
    }

    void Lose()
    {
        mein.GetComponent<Mein>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Key"))
        {
            gotKey = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Door"))
        {
            if(collision.gameObject.GetComponent<DoorOpen>().isOpen && canTP)
            {
                collision.gameObject.GetComponent<DoorOpen>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(WaitToTeleport());
            }
            else if(gotKey)
               collision.gameObject.GetComponent<DoorOpen>().Unlock();
        }
    }

    IEnumerator WaitToTeleport()
    {
        yield return new WaitForSeconds(2f);
        canTP = true;
    }
}
