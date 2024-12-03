using UnityEngine;
using UnityEngine.UI;
using System;


public class HealthUI : MonoBehaviour
{
    public Slider healthBar; // Reference to the health bar slider

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth; // Set slider value
        }
    }
}
