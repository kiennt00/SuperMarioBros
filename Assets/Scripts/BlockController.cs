using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] int numberOfBounces;
    private float bounceSpeed = 0.01f;
    private float bounceHeightDifference = 0.5f;

    private Vector3 originalPosition;

    [SerializeField] BlockType blockType;
    [SerializeField] GameObject emptyBlock;

    public void Bounce(int level)
    {
        originalPosition = transform.position;

        switch (blockType)
        {
            case BlockType.NormalBlock:
                NormalBlockHandle(level);
                break;
            case BlockType.ItemBlock:
                ItemBlockHandle();
                break;
            default: 
                break;
        }
    }

    IEnumerator AnimationBlockBounce()
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

        if (numberOfBounces <= 0)
        {
            gameObject.SetActive(false);
            emptyBlock.SetActive(true);
        }
    }

    private void NormalBlockHandle (int level)
    {
        if (level == 0)
        {
            StartCoroutine(AnimationBlockBounce());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void ItemBlockHandle ()
    {
        numberOfBounces--;
        StartCoroutine(AnimationBlockBounce());
    }
}
