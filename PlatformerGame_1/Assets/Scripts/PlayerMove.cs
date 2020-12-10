using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D playerRB;
    public float moveSpeed;
    float jumpForce = 14f;
    float horizontalMove;
    bool isGrounded;
    public Transform groundCheck;
    Animator animator;
    int maxHP = 10;
    int currentHP;
    bool isHit = false;
    SpriteRenderer spriteRenderer;
    public Mein mein;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        CheckGround();
        if(horizontalMove == 0 && isGrounded)
        {
            animator.SetInteger("State", 0);
        } else
        {
            if (isGrounded)
            {
                FlipPlayer();
                animator.SetInteger("State", 1);
            }
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
        if(deltaHP < 0f)
        {
            StopCoroutine(OnHit());
            isHit = true;
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
        if(isHit)
        {
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g - 0.04f, spriteRenderer.color.b - 0.04f);
        }else
        {
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g + 0.04f, spriteRenderer.color.b + 0.04f);
        }
        if (spriteRenderer.color.g == 1f)
            StopCoroutine(OnHit());
        if (spriteRenderer.color.g <= 0)
            isHit = false;
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    void Lose()
    {
        mein.GetComponent<Mein>().Lose();
    }
}
