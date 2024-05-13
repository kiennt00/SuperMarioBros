using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : BaseMove
{
    public override void OnDead(bool isRightColliding)
    {
        base.OnDead(isRightColliding);
        Destroy(gameObject);
    }
}
