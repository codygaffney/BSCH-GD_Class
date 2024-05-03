using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAGoblinAggro : MonoBehaviour
{
    Animator goblinAnimator;

    void Start()
    {
        // Assuming there is only one Goblin, otherwise, you'll need a different way to ensure you reference the right one.
        goblinAnimator = GameObject.FindObjectOfType<FAGoblin>().GetComponent<Animator>();
        if (goblinAnimator == null)
        {
            Debug.LogError("Goblin's Animator not found!");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && goblinAnimator != null)
        {
            goblinAnimator.SetBool("IsRunning", true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && goblinAnimator != null)
        {
            goblinAnimator.SetBool("IsRunning", false);
        }
    }
}
