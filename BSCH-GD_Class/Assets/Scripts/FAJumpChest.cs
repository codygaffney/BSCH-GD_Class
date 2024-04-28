using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAJumpChest : MonoBehaviour
{
    public FAUnlockNotification unlockNotification;  // Reference to the UnlockNotification script

    // Function to open chest now includes a collider parameter to check the tag of the interacting object
    public void OpenChest(Collider2D collider)
    {
        string unlockedItem = "";  // Default empty string for the unlocked item

        // Determine the unlocked item based on the tag of the object
        switch (collider.tag)
        {
            case "JumpBoostItem":
                unlockedItem = "Jump";
                break;
            case "DoubleJumpItem":
                unlockedItem = "Double Jump";  // Set the item name for double jump
                break;
            case "DashItem":
                unlockedItem = "Dash";  // Set the item name for dash
                break;
            default:
                unlockedItem = "Unknown Item";  // Fallback for unrecognized tags
                break;
        }

        // Check if the notification script is assigned
        if (unlockNotification != null)
        {
            unlockNotification.DisplayUnlock(unlockedItem);  // Display the unlock notification
        }
        else
        {
            Debug.LogError("UnlockNotification script not assigned to ChestController.");
        }
    }
}