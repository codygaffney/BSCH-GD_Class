using UnityEngine;

public class CampfireTrigger : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    public bool isActive;
    GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject");
        }
    }

    // OnTriggerEnter is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Activate the campfire
            animator.SetBool("IsActive", true);
            gameManager.GetComponent<FAGameManager>().spawnPoint = transform;
            gameManager.GetComponent<FAGameManager>().health = gameManager.GetComponent<FAGameManager>().maxHealth; ;
        }
    }
}
