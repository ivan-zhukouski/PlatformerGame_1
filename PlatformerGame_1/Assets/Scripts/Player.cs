using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player: MonoBehaviour
{
    Rigidbody2D playerRB;
    public float moveSpeed;
    float jumpForce = 14f;
    float horizontalMove;
    float verticalMove;
    bool isGrounded = true;
    public Transform groundCheck;
    Animator animator;
    int maxHP = 3;
    int currentHP;
    SpriteRenderer spriteRenderer;
    public Main mein;
    float waitTime = 0.02f;
    Color defaultColor;
    bool gotKey = false;
    bool canTP = true;
    public bool isSwimming = false;
    bool isClimbing = false;
    bool canHit = true;
    int coins = 0;
    public GameObject blueGem, greenGem;
    int gemCount = 0;
    bool isBlueGem = false;
    bool isGreenGem = false;
    [SerializeField] GameObject globalLighting;
    [SerializeField] GameObject doorOneTorch;
    [SerializeField] GameObject doorTwoTorch;
    [SerializeField] GameObject hideRoomOne;
    [SerializeField] GameObject hideRoomTwo;
    [SerializeField] GameObject hideWaterCave;
    [SerializeField] GameObject waterTorches;
    [SerializeField] GameObject hideCave;
    [SerializeField] GameObject caveTorches;
    int stars = 0;


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
        verticalMove = Input.GetAxis("Vertical");

        if (isSwimming && !isClimbing)
        {
            isGrounded = true;
            animator.SetInteger("State", 3);
            if (horizontalMove != 0)
                FlipPlayer();
        }
        else
        {
            CheckGround();
            if (horizontalMove == 0 && isGrounded && !isClimbing)
            {
                animator.SetInteger("State", 0);
            }
            else
            {
                if (isGrounded && !isClimbing)
                {
                    animator.SetInteger("State", 1);
                }
            }
            if (!isGrounded || isGrounded)
            {
                FlipPlayer();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isClimbing)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(horizontalMove * moveSpeed, playerRB.velocity.y);
        
    }

    void Climbing()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, verticalMove * moveSpeed * 0.5f);
    }

    void FlipPlayer()
    {
        if(horizontalMove > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (gemCount == 2)
            {
                blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
                greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
            }

        }
        if(horizontalMove < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
            if(gemCount == 2)
            {
                blueGem.transform.localPosition = new Vector3(0.5f, 0.5f, blueGem.transform.localPosition.z);
                greenGem.transform.localPosition = new Vector3(-0.5f, 0.5f, greenGem.transform.localPosition.z);
            }
            
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
        if(!isGrounded && !isClimbing)
        {
            animator.SetInteger("State", 2);
        }
    }

    public void RecountHP(int deltaHP)
    {
        if(canHit)
            currentHP += deltaHP;
        if(deltaHP < 0 && canHit)
        {
            StartCoroutine(OnHit());
        } else if(currentHP > maxHP)
        {
            currentHP = maxHP;
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
        mein.GetComponent<Main>().LoseGame();
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
        if (collision.gameObject.CompareTag("Coin"))
        {
            coins++;
            Destroy(collision.gameObject);
            print(coins);
        }
        if (collision.gameObject.CompareTag("Heart"))
        {
            RecountHP(1);
            Destroy(collision.gameObject);
            print("Heart " + currentHP);
        }
        if (collision.gameObject.CompareTag("BadMushroom"))
        {
            RecountHP(-1);
            Destroy(collision.gameObject);
            print("Heart " + currentHP);
        }
        if (collision.gameObject.CompareTag("BlueGem") && !isBlueGem)
        {
            isBlueGem = true;
            Destroy(collision.gameObject);
            StartCoroutine(Immortal());
        }
        if (collision.gameObject.CompareTag("GreenGem") && !isGreenGem )
        {
            isGreenGem = true;
            Destroy(collision.gameObject);
            StartCoroutine(DoubleSpeed());
        }
        if(collision.gameObject.CompareTag("DoorOneTrigger"))
        {
            doorOneTorch.gameObject.SetActive(true);
            hideRoomOne.gameObject.SetActive(false);
            StartCoroutine(GlobalLightOff(globalLighting.gameObject.GetComponent<Light2D>(), 0.02f));
        }
        if(collision.gameObject.CompareTag("DoorTwoTrigger"))
        {
            doorTwoTorch.gameObject.SetActive(true);
            hideRoomTwo.gameObject.SetActive(false);
            StartCoroutine(GlobalLightOff(globalLighting.gameObject.GetComponent<Light2D>(), 0.02f));
        }
        if(collision.gameObject.CompareTag("LightOn"))
        {
            if(globalLighting.gameObject.GetComponent<Light2D>().intensity <0.8f)
            {
                StartCoroutine(GlobalLightOn(globalLighting.gameObject.GetComponent<Light2D>(), 0.02f));
            }
        }
        if (collision.gameObject.CompareTag("WaterCaveTrigger"))
        {
            hideWaterCave.gameObject.SetActive(false);
            waterTorches.gameObject.SetActive(true);
            StartCoroutine(GlobalLightOff(globalLighting.gameObject.GetComponent<Light2D>(), 0.02f));
        }
        if (collision.gameObject.CompareTag("CaveTrigger"))
        {
            hideCave.gameObject.SetActive(false);
            caveTorches.gameObject.SetActive(true);
            StartCoroutine(GlobalLightOff(globalLighting.gameObject.GetComponent<Light2D>(), 0.02f));
        }
        if(collision.gameObject.CompareTag("Star"))
        {
            stars++;
            Destroy(collision.gameObject);
        }
    }

    IEnumerator GlobalLightOff(Light2D lg, float time)
    {
        lg.intensity -= time * 2;
        
        yield return new WaitForSeconds(time);
        if (lg.intensity > 0)
        {
            StartCoroutine(GlobalLightOff(lg, time));
        }
        else
        {
            StopCoroutine(GlobalLightOff(lg, time));
        }
        print(lg.intensity);
        
       
    }

    IEnumerator GlobalLightOn(Light2D lg, float time)
    {
        lg.intensity += time * 2;

        yield return new WaitForSeconds(time);
        if (lg.intensity < 0.8f)
        {
            StartCoroutine(GlobalLightOn(lg, time));
        }
    }

    IEnumerator WaitToTeleport()
    {
        yield return new WaitForSeconds(2f);
        canTP = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isClimbing = true;
            playerRB.gravityScale = 0f;
            if (Input.GetAxis("Vertical") == 0)
            {
                animator.SetInteger("State", 4);  
                playerRB.velocity = new Vector2(playerRB.velocity.x, verticalMove);
            }
            else
            {
                animator.SetInteger("State", 5);
                Climbing();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isClimbing = false;
            playerRB.gravityScale = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Trampoline"))
        {
            StartCoroutine(TrampolineAnimation(collision.gameObject.GetComponentInParent<Animator>()));
            Debug.Log("jump");
        }
    }

    IEnumerator TrampolineAnimation(Animator anim)
    {
        anim.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isJump", false);

    }

    IEnumerator Immortal()
    {
        gemCount++;
        canHit = false;
        blueGem.SetActive(true);
        CheckGem(blueGem);
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        
        yield return new WaitForSeconds(4f);
        StartCoroutine(GemInvis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);

        canHit = true;
        blueGem.SetActive(false);
        gemCount--;
        CheckGem(greenGem);
        isBlueGem = false;
    }

    IEnumerator DoubleSpeed()
    {
        gemCount++;
        greenGem.SetActive(true);
        moveSpeed *= 2;
        CheckGem(greenGem);
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(4f);
        StartCoroutine(GemInvis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);

        moveSpeed /= 2;
        gemCount--;
        greenGem.SetActive(false);
        CheckGem(blueGem);
        isGreenGem = false;
    }

    void CheckGem(GameObject obj)
    {
        if(gemCount == 1)
        {
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        }
        else if(gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
        }
    }

    IEnumerator GemInvis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if(spr.color.a > 0)
        {
            StartCoroutine(GemInvis(spr, time));
        }
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetHP()
    {
        return currentHP;
    }
}
