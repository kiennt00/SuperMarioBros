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

    [SerializeField] bool coin = false;
    [SerializeField] bool magicMushroomOrFireFlower = false;
    [SerializeField] bool starman = false;
    [SerializeField] bool oneUpMushroom = false;

    public void Bounce(int level)
    {
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

        if (magicMushroomOrFireFlower)
        {
            GameObject item = null;
            if (level == 0)
            {
                item = (GameObject) Instantiate(Resources.Load("Prefabs/Magic Mushroom"));
            }
            else
            {
                item = (GameObject)Instantiate(Resources.Load("Prefabs/Fire Flower"));
            }
            item.transform.SetParent(transform.parent, true);
            item.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
        }

        if (coin)
        {
            Coin coin = (Coin)SimplePool.Spawn(PoolType.Coin, transform.position, transform.rotation);
            coin.coinBounce();
        }

        if(starman)
        {
            GameObject item = (GameObject)Instantiate(Resources.Load("Prefabs/Starman"));
            item.transform.SetParent(transform.parent, true);
            item.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
        }

        if (oneUpMushroom)
        {
            GameObject item = (GameObject)Instantiate(Resources.Load("Prefabs/One Up Mushroom"));
            item.transform.SetParent(transform.parent, true);
            item.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
        }
    }
}
