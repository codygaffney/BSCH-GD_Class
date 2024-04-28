using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAUIFollow : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the inspector
    public GameObject worldSpaceUI; // Assign the world space UI GameObject in the inspector
    public Vector3 offset; // Offset from the player's position

    void Update()
    {
        if (player != null && worldSpaceUI != null)
        {
            // Set the UI position to be above the player with the specified offset
            worldSpaceUI.transform.position = player.position + offset;
        }
    }
}
