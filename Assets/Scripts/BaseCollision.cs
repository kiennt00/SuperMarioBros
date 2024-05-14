using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollision : MonoBehaviour
{
    [SerializeField] public BaseMove baseMove;
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }

        if ((collision.contacts[0].normal.x > 0 || collision.contacts[0].normal.x < 0) && !collision.collider.CompareTag("Player"))
        {
            baseMove.ChangeDirection();
        }
    }
}
