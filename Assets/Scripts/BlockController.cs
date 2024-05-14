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
    [SerializeField] BlockContainItem blockContainItem;
    [SerializeField] GameObject emptyBlock;
    [SerializeField] GameObject magicMushroom;
    [SerializeField] GameObject fireFlower;
    [SerializeField] GameObject starman;
    [SerializeField] GameObject oneUpMushroom;

    public void Bounce(int level)
    {
        if (numberOfBounces <= 0) return;

        originalPosition = transform.position;

        switch (blockType)
        {
            case BlockType.NormalBlock:
                NormalBlockHandle(level);
                break;

            case BlockType.ItemBlock:
                ItemBlockHandle(level);
                break;

            default: 
                break;
        }
    }

    IEnumerator IEBlockBounce()
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
            StartCoroutine(IEBlockBounce());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void ItemBlockHandle (int level)
    {
        numberOfBounces--;
        StartCoroutine(IEBlockBounce());

        GameObject item = null;
        switch (blockContainItem)
        {
            case BlockContainItem.Coin:
                Coin coin = (Coin)SimplePool.Spawn(PoolType.Coin, transform.position, transform.rotation);
                coin.coinBounce();
                break;

            case BlockContainItem.MagicMushroomOrFireFlower:
                if (level == 0)
                {
                    item = Instantiate(magicMushroom);
                }
                else
                {
                    item = Instantiate(fireFlower);
                }
                item.transform.SetParent(transform.parent, true);
                item.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
                break;

            case BlockContainItem.Starman:
                item = Instantiate(starman);
                item.transform.SetParent(transform.parent, true);
                item.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
                break;

            case BlockContainItem.OneUpMushroom:
                item = Instantiate(oneUpMushroom);
                item.transform.SetParent(transform.parent, true);
                item.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
                break;

            default:
                break;
        }
    }
}
