using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] Transform playerTransform;
    private readonly float minCameraPositionX = -21;
    private readonly float maxCameraPositionX = 163;
    [SerializeField] Transform rightEdge;
    public float RightPoint => rightEdge.position.x;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 cameraPosition = transform.position;
            cameraPosition.x = playerTransform.position.x;
            if (cameraPosition.x > maxCameraPositionX)
            {
                cameraPosition.x = maxCameraPositionX;
            }
            if (cameraPosition.x < minCameraPositionX)
            {
                cameraPosition.x = minCameraPositionX;
            }
            transform.position = cameraPosition;
        }
    }
}
