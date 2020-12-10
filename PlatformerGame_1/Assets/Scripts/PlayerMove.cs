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

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        Debug.Log(isGrounded);
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(horizontalMove * moveSpeed, playerRB.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
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
        playerRB.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); 
    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        
        isGrounded = colliders.Length > 1;
        if(!isGrounded)
        {
            animator.SetInteger("State", 2);
            FlipPlayer();
        }
    }
}
