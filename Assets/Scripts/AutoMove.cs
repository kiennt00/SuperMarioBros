using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.05f;
    [SerializeField] bool isFacingRight = false;
    [SerializeField] GameObject gameObjectTarget;

    private void FixedUpdate()
    {
        Vector2 newPosition = gameObjectTarget.transform.position;

        if (isFacingRight)
        {
            newPosition.x += moveSpeed;
        }
        else
        {
            newPosition.x -= moveSpeed;
        }

        gameObjectTarget.transform.position = newPosition;
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

        if (collision.collider.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            // dead
        }
    }

    void ChangeDirection()
    {
        isFacingRight = !isFacingRight;
        Vector2 direction = gameObjectTarget.transform.eulerAngles;
        direction.y += 180f;
        gameObjectTarget.transform.eulerAngles = direction;
    }
}
