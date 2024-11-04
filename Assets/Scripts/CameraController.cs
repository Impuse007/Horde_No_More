using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float left;
    public float right;
    public float bottom;
    public float top;

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        float clampedX = Mathf.Clamp(smoothedPosition.x, -left, right);
        float clampedY = Mathf.Clamp(smoothedPosition.y, -bottom, top);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
