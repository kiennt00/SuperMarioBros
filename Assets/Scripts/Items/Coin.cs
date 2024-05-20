using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Coin : GameUnit
{
    private float bounceSpeed = 0.01f;
    private float bounceHeightDifference = 3f;

    private Vector3 originalPosition;
    public void coinBounce()
    {
        UIManager.Ins.GetUI<UIGameplay>().AddScore(200);
        originalPosition = transform.position;
        StartCoroutine(IECoinBounce());
    } 

    IEnumerator IECoinBounce()
    {
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + bounceSpeed);
            if (transform.position.y >= originalPosition.y + bounceHeightDifference) break;
            yield return null;
        }

        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - bounceSpeed);
            if (transform.position.y <= originalPosition.y) break;
            yield return null;
        }

        SimplePool.Despawn(this);
    }
}
