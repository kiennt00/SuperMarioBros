using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public LayerMask groundLayer;

    [SerializeField] BoxCollider2D playerBoxCollider;

    private float raycastPosition;
    public float raycastDistance = 0.1f;

    public bool isOnGround;

    void Update()
    {
        raycastPosition = playerBoxCollider.size.x / 2f;

        RaycastHit2D hitGroundLeft = Physics2D.Raycast(transform.position - new Vector3(raycastPosition, 0f, 0f), -Vector2.up, raycastDistance, groundLayer);
        RaycastHit2D hitGroundRight = Physics2D.Raycast(transform.position + new Vector3(raycastPosition, 0f, 0f), -Vector2.up, raycastDistance, groundLayer);

        if (hitGroundLeft.collider != null || hitGroundRight.collider != null)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }
}
