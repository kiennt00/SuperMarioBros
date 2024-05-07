using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public bool canBounce = true;
    public int numberOfBounces = 1;
    public float bounceSpeed = 0.01f;
    public float bounceHeightDifference = 0.5f;

    public Vector3 originalPosition;

    [SerializeField] BlockType blockType;


    public void Bounce(bool isBreaking)
    {
        if (canBounce)
        {
            if (blockType == BlockType.NormalBlock && !isBreaking)
            {
                numberOfBounces++;
            }

            originalPosition = transform.position;
            numberOfBounces--;

            if (numberOfBounces == 0)
            {
                canBounce = false;
            }

            StartCoroutine(AnimationBlockBounce());

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

        if (blockType == BlockType.NormalBlock && !canBounce)
        {
            gameObject.SetActive(false);
        } 
        else
        {
            while (true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - bounceSpeed);
                if (transform.position.y <= originalPosition.y) break;
                yield return null;
            }
        }  
    }

    public void NormalBlockHandle ()
    {
        numberOfBounces++;
    }

    public void QuestionBlockHandle ()
    {

    }
}
