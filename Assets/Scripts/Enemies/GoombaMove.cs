using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMove : BaseMove
{
    [SerializeField] public GameObject objectAlive, objectDead;
    public IEnumerator IEDead()
    {
        objectAlive.SetActive(false);
        objectDead.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    public override void OnDead(bool isRightColliding)
    {
        base.OnDead(isRightColliding);
        StartCoroutine(IEDead());
    }
}
