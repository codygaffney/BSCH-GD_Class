using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public float scoreValue;
    public GameManagerScript gameManagerScript;
    public GameObject collectedEffect;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManagerScript.AddScore(scoreValue);
            Instantiate(collectedEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
