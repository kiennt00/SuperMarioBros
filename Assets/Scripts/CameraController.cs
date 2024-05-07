using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    private readonly float minCameraPositionX = 3;
    private readonly float maxCameraPositionX = 187;


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
