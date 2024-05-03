using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FASpikePit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
            gameManager.GetComponent<FAGameManager>().TakeDamage(gameManager.GetComponent<FAGameManager>().health);
        }
    }
}
