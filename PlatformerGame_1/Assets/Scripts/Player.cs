using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

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
    int currentStar = 0;
    SpriteRenderer spriteRenderer;
    public Main main;
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
    public bool isBlueGem = false;
    public bool isGreenGem = false;
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

    public Inventory inventory;
    public SoundEffector soundEffector;
    public Joystick joystick;


    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        if(!PlayerPrefs.HasKey("Star"))
        {
            PlayerPrefs.SetInt("Star", currentStar);
        }
    }

    void Update()
    {
        horizontalMove = joystick.Horizontal;
        verticalMove = joystick.Vertical;

        if (isSwimming && !isClimbing)
        {
            isGrounded = true;
            animator.SetInteger("State", 3);

            if (horizontalMove >= 0.2f || horizontalMove <= -0.2f)
                FlipPlayer();
        }
        else
        {
            CheckGround();
            if (horizontalMove < 0.2f && horizontalMove > -0.2f && isGrounded && !isClimbing)
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
    }

    void FixedUpdate()
    {
        if(horizontalMove >= 0.2f)
        playerRB.velocity = new Vector2(moveSpeed, playerRB.velocity.y);
        else if (horizontalMove <= -0.2f)
            playerRB.velocity = new Vector2(-moveSpeed, playerRB.velocity.y);
        else
        {
            playerRB.velocity = new Vector2(0f, playerRB.velocity.y);
        }

    }

    void Climbing()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, verticalMove * moveSpeed * 0.5f);
    }

    void FlipPlayer()
    {
        if(horizontalMove >= 0.2f)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (gemCount == 2)
            {
                blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
                greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
            }

        }
        if(horizontalMove <= -0.2f)
        {
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
            if(gemCount == 2)
            {
                blueGem.transform.localPosition = new Vector3(0.5f, 0.5f, blueGem.transform.localPosition.z);
                greenGem.transform.localPosition = new Vector3(-0.5f, 0.5f, greenGem.transform.localPosition.z);
            }
            
        }
        
    }

    public void Jump()
    {
        if(isGrounded && !isClimbing)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            soundEffector.Play_jumpSound();
        }
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
        soundEffector.Play_loseSound();
        main.GetComponent<Main>().LoseGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Key"))
        {
            gotKey = true;
            inventory.Add_key();
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
            soundEffector.Play_coinSound();
            coins++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Heart"))
        {
            inventory.Add_hp();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("BadMushroom"))
        {
            RecountHP(-1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("BlueGem"))
        {
            inventory.Add_blueGem();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("GreenGem"))
        {
            inventory.Add_greenGem();
            Destroy(collision.gameObject);
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
        if(collision.gameObject.CompareTag("DeadZone"))
        {
            Lose();
        }
        if(collision.gameObject.CompareTag("Star"))
        {
            soundEffector.Play_starSound();
            currentStar++;
            print(PlayerPrefs.GetInt("Star"));
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
            if (verticalMove == 0)
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
        isBlueGem = true;
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
        isGreenGem = true;
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

    public void ActiveGreenGem()
    {
        StartCoroutine(DoubleSpeed());
    }

    public void ActiveBlueGem()
    {
        StartCoroutine(Immortal());
    }

    public int GetStar()
    {
        return currentStar;
    }
}
