using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : BaseMove
{
    public override void OnDead()
    {
        base.OnDead();
        Destroy(gameObject);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("Player"))
        {
            OnDead();
        }
    }
}
