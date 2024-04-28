using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FAUnlockNotification : MonoBehaviour
{
    public TextMeshProUGUI notificationText;  // Reference to the TextMeshPro component
    public float displayDuration = 2.5f;  // Duration the notification is visible

    private void Awake()
    {
        gameObject.SetActive(false);  // Hide the notification at start
    }

    public void DisplayUnlock(string item)
    {
        notificationText.text = item + " Unlocked!";  // Set the text to show what was unlocked
        gameObject.SetActive(true);  // Show the notification
        Invoke("HideNotification", displayDuration);  // Set a timer to hide the notification
    }

    void HideNotification()
    {
        gameObject.SetActive(false);  // Hide the notification
    }
}
