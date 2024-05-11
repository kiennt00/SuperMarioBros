using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : BaseMove
{
    [SerializeField] public GameObject objectAlive, objectDead;
    public IEnumerator AnimationDead()
    {
        objectAlive.SetActive(false);
        objectDead.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    public override void OnDead(bool isRightColliding)
    {
        base.OnDead(isRightColliding);
        StartCoroutine(AnimationDead());
    }
}
