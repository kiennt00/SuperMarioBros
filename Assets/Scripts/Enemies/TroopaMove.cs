using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopaMove : BaseMove
{
    [SerializeField] GameObject objectAlive, objectDead, objectRevive;
    [SerializeField] BoxCollider2D boxCollider;
    float reviveTime = 10f, firstTouchTime, duration;

    private bool canRevive = true;

    Coroutine coroutine;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!canRevive)
        {
            moveSpeed = 0.2f;
            Vector2 newPosition = gameObject.transform.position;

            if (isFacingRight)
            {
                newPosition.x += moveSpeed;
            }
            else
            {
                newPosition.x -= moveSpeed;
            }

            gameObject.transform.position = newPosition;
        }
    }

    protected override void OnDead(bool isRightColliding)
    {
        base.OnDead(isRightColliding);
        if (canRevive)
        {
            if (firstTouchTime == 0)
            {
                firstTouchTime = Time.time;
            }
            else
            {
                duration = Time.time - firstTouchTime;
                firstTouchTime = Time.time;
            }

            if (duration != 0 && duration < reviveTime * 2)
            {
                StopCoroutine(coroutine);
                AfterDeadHandle(isRightColliding);
            }
            else
            {
                coroutine = StartCoroutine(IEDead());
            }
        }
    }

    IEnumerator IEDead()
    {
        objectAlive.SetActive(false);
        objectDead.SetActive(true);

        boxCollider.offset = new Vector2(0f, 0.4375f);
        boxCollider.size = new Vector2(1f, 0.875f);

        yield return new WaitForSeconds(reviveTime);

        coroutine = StartCoroutine(IERevive());
    }

    IEnumerator IERevive()
    {
        objectDead.SetActive(false);
        objectRevive.SetActive(true);

        boxCollider.offset = new Vector2(0f, 0.46875f);
        boxCollider.size = new Vector2(1f, 0.9375f);

        yield return new WaitForSeconds(reviveTime);

        objectRevive.SetActive(false);
        objectAlive.SetActive(true);

        boxCollider.offset = new Vector2(0f, 0.75f);
        boxCollider.size = new Vector2(1f, 1.5f);

        isDead = false;
    }

    void AfterDeadHandle(bool isRightColliding)
    {
        UIManager.Ins.GetUI<UIGameplay>().AddScore(100);

        canRevive = false;

        if (isRightColliding)
        {
            isFacingRight = false;
        }
        else
        {
            isFacingRight = true;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            UIManager.Ins.GetUI<UIGameplay>().AddScore(100);

            if (collision.contacts[0].normal.x < 0)
            {
                OnDead(true);
            }
            else
            {
                OnDead(false);
            }
        }
    }

    private void Update()
    {
        if (!firstSeen && CameraController.Ins.RightPoint > transform.position.x)
        {
            firstSeen = true;
        }
    }
}
