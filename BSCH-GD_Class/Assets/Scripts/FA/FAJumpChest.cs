using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FAJumpChest : MonoBehaviour
{
    public FAUnlockNotification unlockNotification;  // Reference to the UnlockNotification script
    public GameObject promptDisplay;
    public Vector3 promptOffset = new Vector3(0, 2.0f, 0);

    public FAGameManager gameManager;


    void Start()
    {
        gameManager = FindAnyObjectByType<FAGameManager>();
        if (promptDisplay != null)
        {
            promptDisplay.SetActive(false);  // Initially hide the prompt
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.CompareTag("DoubleJumpItem") && !gameManager.IsGoblinDead)
        {
            
            promptDisplay.GetComponentInChildren<TMP_Text>().text = "Locked!";
            UpdatePromptPosition();
            DisplayPrompt(true);
        }
        else if (this.CompareTag("DashItem") && !gameManager.IsSkeletonDead)
        {
            promptDisplay.GetComponentInChildren<TMP_Text>().text = "Locked!";
            UpdatePromptPosition();
            DisplayPrompt(true);
        }
        else
        {
            promptDisplay.GetComponentInChildren<TMP_Text>().text = "Press 'F'";
            UpdatePromptPosition();
            DisplayPrompt(true);
            Debug.Log("Press F");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DisplayPrompt(false);
        }
    }

    void DisplayPrompt(bool show)
    {
        if (promptDisplay != null)
        {
            promptDisplay.SetActive(show);
        }
    }

    void UpdatePromptPosition()
    {
        if (promptDisplay != null)
        {
            Vector3 chestTop = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            promptDisplay.transform.position = chestTop + promptOffset;  // Position the prompt above the chest
        }
    }

    // Function to open chest now includes a collider parameter to check the tag of the interacting object
    public void OpenChest()
    {
        if (this.CompareTag("DoubleJumpItem") && !gameManager.IsGoblinDead)
        {
            Debug.Log("Double Jump is locked. Defeat the Goblin first!");
            return;  // Prevent opening the chest if the Goblin is not defeated
        }
        else if(this.CompareTag("DashItem") && !gameManager.IsSkeletonDead)
        {
            return;
        }

        string unlockedItem = "";  // Default empty string for the unlocked item

        // Determine the unlocked item based on the tag of the object
        switch (this.tag)
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