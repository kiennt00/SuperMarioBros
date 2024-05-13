using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : BaseCollision
{
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("Player"))
        {
            baseMove.OnDead(true);
        }
    }
}
