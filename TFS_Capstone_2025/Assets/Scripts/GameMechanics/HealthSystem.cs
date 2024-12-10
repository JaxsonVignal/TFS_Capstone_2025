using UnityEngine;
using UnityEngine.Events; // For events (optional)

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // Maximum health
    public float currentHealth;     // Current health
    public bool isDead = false;     // Is the object dead?

    // Events for health changes (optional, can be hooked in Unity inspector)
    public UnityEvent onHealthChanged;
    public UnityEvent onDeath;

    private void Awake()
    {
        // Initialize health - must be set before start
        currentHealth = maxHealth;
    }

    private void Start()
    {

    }

    // Method to deal damage
    public void TakeDamage(float amount)
    {
        if (isDead) return;  // Don't take damage if already dead

        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        // Trigger health changed event
        onHealthChanged?.Invoke();

        // Check for death
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // Method to heal
    public void Heal(float amount)
    {
        if (isDead) return;  // Don't heal if dead

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        // Trigger health changed event
        onHealthChanged?.Invoke();
    }

    // Method to reset health (optional)
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        // Trigger health changed event
        onHealthChanged?.Invoke();
    }

    // Method to handle death
    private void Die()
    {
        isDead = true;
        onDeath?.Invoke();

        // Add any other death logic here, such as playing an animation, destroying the object, etc.
        Debug.Log($"{gameObject.name} has died.");
    }

    // Method to get the health percentage (0 to 1)
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    // Optional method to check if alive
    public bool IsAlive()
    {
        return !isDead;
    }
}
