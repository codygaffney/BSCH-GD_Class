using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIScript : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public GameManagerScript gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + gameManagerScript.score;
        healthText.text = "Health: " + gameManagerScript.health;
    }
}
