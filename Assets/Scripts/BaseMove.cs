using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    protected float moveSpeed = 0.05f;
    [SerializeField ]protected bool isFacingRight = false;

    protected bool isDead = false;
    [SerializeField] protected bool firstSeen = false;

    protected virtual void FixedUpdate()
    {
        if (!isDead && firstSeen)
        {
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

    protected virtual void ChangeDirection()
    {
        isFacingRight = !isFacingRight;
        Vector2 direction = gameObject.transform.eulerAngles;
        direction.y += 180f;
        gameObject.transform.eulerAngles = direction;
    }

    protected virtual void OnDead()
    {
        isDead = true;
    }

    protected virtual void OnDead(bool isRightColliding)
    {
        isDead = true;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }

        if ((collision.contacts[0].normal.x > 0 || collision.contacts[0].normal.x < 0) && !collision.collider.CompareTag("Player"))
        {
            ChangeDirection();
        }
    }
}
