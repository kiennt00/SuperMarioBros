using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.05f;
    [SerializeField] bool isFacingRight = false;
    [SerializeField] Transform spriteTransform;
    private void FixedUpdate()
    {
        Vector2 newPosition = transform.position;

        if (isFacingRight)
        {
            newPosition.x += moveSpeed;
        }
        else
        {
            newPosition.x -= moveSpeed;
        }

        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("DeathZone"))
        {
            gameObject.SetActive(false);
        }

        if ((collision.contacts[0].normal.x > 0 || collision.contacts[0].normal.x < 0) && !collision.collider.CompareTag("Player"))
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        isFacingRight = !isFacingRight;
        Vector2 direction = spriteTransform.eulerAngles;
        direction.y += 180f;
        spriteTransform.eulerAngles = direction;
    }
}
