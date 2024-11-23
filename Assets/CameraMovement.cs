using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour //this is a camera movement script so the camera can follow the player smoothly
{
    public Transform player;  // Reference to the player's transform
    public float smoothSpeed = 0.125f;  // Smoothing speed
    public Vector3 offset;  // Offset between the player and the camera

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}