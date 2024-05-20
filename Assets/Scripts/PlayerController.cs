using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerSprite, playerDeadByEnemy;
    [SerializeField] AudioSource audioSource;
    [SerializeField] CircleCollider2D headCollider;
    [SerializeField] Rigidbody2D r2d;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D boxCollider;

    [SerializeField] PlayerRaycast playerRaycast;

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
    private bool isImmune = false;
    private bool isTransforming = false;

    private int level = 0;

    private Vector3 deadPosition;


    // Update is called once per frame
    void Update()
    {
        isOnGround = playerRaycast.isOnGround;

        animator.SetFloat("currentSpeed", currentSpeed);
        animator.SetBool("isOnGround", isOnGround);
        animator.SetBool("isChangingDirection", isChangingDirection);

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            Jump();
        }

        SprintAndFire();

        if (isTransforming)
        {
            switch (level)
            {
                case 0:
                    {
                        StartCoroutine(IEBecomeSmall());
                        headCollider.offset = new Vector2(0, 1);
                        isTransforming = false;
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(IEBecomeBig());
                        headCollider.offset = new Vector2(0, 2);
                        isTransforming = false;
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(IEBecomeSpecial());
                        isTransforming = false;
                        break;
                    }
                default:
                    isTransforming = false;
                    break;
            }
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
        Vector2 direction = playerSprite.transform.eulerAngles;
        direction.y += 180f;
        playerSprite.transform.eulerAngles = direction;
        if (currentSpeed > 0 && isOnGround)
        {
            StartCoroutine(IEChangeDirection());
        }
    }

    void Jump()
    {
        PlaySound("smb_jump-small");
        r2d.velocity = new Vector2(r2d.velocity.x, jumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            collision.gameObject.GetComponent<BlockController>().Bounce(level);
        }
    }

    IEnumerator IEChangeDirection()
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

    IEnumerator IEBecomeBig()
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

    IEnumerator IEBecomeSpecial()
    {
        float delay = 0.1f;
        PlaySound("smb_powerup");
        animator.SetLayerWeight(animator.GetLayerIndex("Small"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("Big"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("Special"), 1);
        yield return new WaitForSeconds(delay);
    }

    IEnumerator IEBecomeSmall()
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

    IEnumerator IEDead()
    {
        float speed = 0.015f, height = 2.5f;
        while (true)
        {
            playerDeadByEnemy.transform.position = new Vector2(playerDeadByEnemy.transform.position.x,  playerDeadByEnemy.transform.position.y + speed);
            if (playerDeadByEnemy.transform.position.y >= deadPosition.y + height) break;
            yield return null;
        }

        while (true)
        {
            playerDeadByEnemy.transform.position = new Vector2(playerDeadByEnemy.transform.position.x, playerDeadByEnemy.transform.position.y - speed);
            if (playerDeadByEnemy.transform.position.y <= -3f) break;
            yield return null;
        }
    }

    void PlaySound(string fileAudio)
    {
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/" + fileAudio));
    }

    private void DeadByDeadZone()
    {
        PlaySound("smb_mariodie");
        enabled = false;
    }

    private void OnDead()
    {
        PlaySound("smb_mariodie");
        enabled = false;
        r2d.gravityScale = 0f;
        r2d.Sleep();
        boxCollider.enabled = !boxCollider.enabled;
        deadPosition = transform.position;
        playerSprite.gameObject.SetActive(false);
        playerDeadByEnemy.gameObject.SetActive(true);
        StartCoroutine(IEDead());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && !(collision.contacts[0].normal.y > 0))
        {
            if (isImmune) return;

            if (level == 0)
            {
                OnDead();
            }
            else
            {
                level = 0;
                isTransforming = true;
                StartCoroutine(IEImmune());
            }
        }

        if (collision.collider.CompareTag("MagicMushroom"))
        {
            level = 1;
            isTransforming = true;
        }

        if (collision.collider.CompareTag("FireFlower"))
        {
            level = 2;
            isTransforming = true;
        }

        if (collision.collider.CompareTag("DeathZone"))
        {
            DeadByDeadZone();
        }
    }

    IEnumerator IEImmune()
    {
        float immuneDuration = 0.5f;
        isImmune = true;
        yield return new WaitForSeconds(immuneDuration);
        isImmune = false;
    }
}
