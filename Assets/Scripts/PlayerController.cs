using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerSpriteTransform;
    [SerializeField] AudioSource audioSource;
    [SerializeField] CircleCollider2D headCollider;
    [SerializeField] Rigidbody2D r2d;
    [SerializeField] Animator animator;

    private float currentSpeed = 0;
    private float maxSpeed = 6f;
    private readonly float maxSpeedNonHoldingLShift = 6f;
    private readonly float maxSpeedHoldingLShift = 9f;
    private float jumpForce = 18.5f;
    private float timeHoldingLShift = 0;
    private readonly float timeHoldinglShiftMax = 0.2f;

    private bool isChangingDirection = false;
    private bool isOnGround = true;
    private bool isFacingRight = true;
    private bool isDead = false;

    public int level = 0;

    [SerializeField] bool powerUp = false;



    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("currentSpeed", currentSpeed);
        animator.SetBool("isOnGround", isOnGround);
        animator.SetBool("isChangingDirection", isChangingDirection);

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            Jump();
        }

        SprintAndFire();

        if (powerUp)
        {
            switch (level)
            {
                case 0:
                    {
                        StartCoroutine(BecomeSmall());
                        headCollider.offset = new Vector2(0, 1);
                        powerUp = false;
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(BecomeBig());
                        headCollider.offset = new Vector2(0, 2);
                        powerUp = false;
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(BecomeSpecial());
                        powerUp = false;
                        break;
                    }
                default:
                    powerUp = false;
                    break;
            }
        }

        if (!isDead && transform.position.y < -7)
        {
            r2d.gravityScale = 0f;
            r2d.Sleep();
            enabled = false;
            isDead = true;
            PlaySound("smb_mariodie");
        }
    }

    private void FixedUpdate()
    {
        move();
    }

    void move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        r2d.velocity = new Vector2(maxSpeed * horizontal, r2d.velocity.y);
        currentSpeed = Math.Abs(maxSpeed * horizontal);
        if (horizontal > 0 && !isFacingRight) CheckDirection();
        if (horizontal < 0 && isFacingRight) CheckDirection();
    }

    void CheckDirection()
    {
        isFacingRight = !isFacingRight;
        Vector2 direction = playerSpriteTransform.eulerAngles;
        direction.y += 180f;
        playerSpriteTransform.eulerAngles = direction;
        if (currentSpeed > 0 && isOnGround)
        {
            StartCoroutine(ChangeDirection());
        }
    }

    void Jump()
    {
        PlaySound("smb_jump-small");
        r2d.velocity = new Vector2(r2d.velocity.x, jumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Brick"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Brick"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Brick"))
        {
            isOnGround = false;
        }
    }

    IEnumerator ChangeDirection()
    {
        isChangingDirection = true;
        yield return new WaitForSeconds(0.2f);
        isChangingDirection = false;
    }

    void SprintAndFire()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            timeHoldingLShift += Time.deltaTime;
            if (timeHoldingLShift > timeHoldinglShiftMax)
            {
                maxSpeed = maxSpeedHoldingLShift;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            print("shoot!");
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            maxSpeed = maxSpeedNonHoldingLShift;
            timeHoldingLShift = 0;
        }
    }

    IEnumerator BecomeBig()
    {
        float delay = 0.1f;
        PlaySound("smb_powerup");
        for (int i = 0; i < 3; i++)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Small"), 1);
            animator.SetLayerWeight(animator.GetLayerIndex("Big"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Special"), 0);
            yield return new WaitForSeconds(delay);
            animator.SetLayerWeight(animator.GetLayerIndex("Small"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Big"), 1);
            animator.SetLayerWeight(animator.GetLayerIndex("Special"), 0);
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator BecomeSpecial()
    {
        float delay = 0.1f;
        PlaySound("smb_powerup");
        animator.SetLayerWeight(animator.GetLayerIndex("Small"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("Big"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("Special"), 1);
        yield return new WaitForSeconds(delay);
    }

    IEnumerator BecomeSmall()
    {
        float delay = 0.1f;
        PlaySound("smb_pipe");
        for (int i = 0; i < 3; i++)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Small"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Big"), 1);
            animator.SetLayerWeight(animator.GetLayerIndex("Special"), 0);
            yield return new WaitForSeconds(delay);
            animator.SetLayerWeight(animator.GetLayerIndex("Small"), 1);
            animator.SetLayerWeight(animator.GetLayerIndex("Big"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Special"), 0);
            yield return new WaitForSeconds(delay);
        }
    }

    void PlaySound(string fileAudio)
    {
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/" + fileAudio));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Block"))
        {
            if (level == 0)
            {
                collision.gameObject.GetComponent<BlockController>().Bounce(false);
            }
            else
            {
                collision.gameObject.GetComponent<BlockController>().Bounce(true);
            }
        }
    }
}
