using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 0.05f;
    [SerializeField] public bool isFacingRight = false;

    public bool isDead = false;

    public virtual void FixedUpdate()
    {
        if (!isDead)
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

    public virtual void ChangeDirection()
    {
        isFacingRight = !isFacingRight;
        Vector2 direction = gameObject.transform.eulerAngles;
        direction.y += 180f;
        gameObject.transform.eulerAngles = direction;
    }

    public virtual void OnDead(bool isRightColliding)
    {
        isDead = true;
    }
}
