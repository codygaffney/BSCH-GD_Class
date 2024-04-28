using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FAPlayerHealthBar : MonoBehaviour
{
    public Image healthFill;
    public FAGameManager playerHealth;

    private void Update()
    {
        if (playerHealth != null && healthFill != null)
        {
            healthFill.fillAmount = playerHealth.health / playerHealth.maxHealth; 
        }
    }
}
