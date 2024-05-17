using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMove : BaseMove
{
    [SerializeField] public GameObject objectAlive, objectDead;
    [SerializeField] public BoxCollider2D boxCollider2D;
    public IEnumerator IEDead()
    {
        objectAlive.SetActive(false);
        objectDead.SetActive(true);
        boxCollider2D.enabled = false;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    public override void OnDead()
    {
        base.OnDead();
        StartCoroutine(IEDead());
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            OnDead();
        }
    }
}
