using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GoombaMove : BaseMove
{
    [SerializeField] GameObject objectAlive, objectDead;
    [SerializeField] BoxCollider2D boxCollider2D;
    IEnumerator IEDead()
    {
        objectAlive.SetActive(false);
        objectDead.SetActive(true);
        boxCollider2D.enabled = false;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    protected override void OnDead()
    {
        base.OnDead();
        StartCoroutine(IEDead());
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            OnDead();
            UIManager.Ins.GetUI<UIGameplay>().AddScore(100);
        }
    }

    private void Update()
    {
        if (CameraController.Ins.RightPoint > transform.position.x)
        {
            firstSeen = true;
        }
    }
}
