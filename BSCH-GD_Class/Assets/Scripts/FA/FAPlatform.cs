using UnityEngine;

public class FAPlatform2D : MonoBehaviour
{
    public float height = 4.0f;   // The maximum height the platform moves up, visible in the Inspector
    public float speed = 0.5f;    // The speed at which the platform moves, visible in the Inspector

    private Vector2 startPosition;
    private Vector2 targetPosition;

    void Start()
    {
        startPosition = transform.position;  // Store the initial position of the platform
        targetPosition = new Vector2(startPosition.x + height, startPosition.y);  // Calculate the target position
    }

    void Update()
    {
        // Calculate the new position based on the time
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector2.Lerp(startPosition, targetPosition, time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider belongs to the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Make the player a child of the platform
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the collider belongs to the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Detach the player from the platform
            collision.transform.SetParent(null);
        }
    }
}
