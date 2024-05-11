using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaCollision : BaseCollision
{
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            if (collision.contacts[0].normal.x < 0)
            {
                baseMove.OnDead(true);
            }
            else
            {
                baseMove.OnDead(false);
            }
        }
    }
}
