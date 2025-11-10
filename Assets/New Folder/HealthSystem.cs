using System.ComponentModel;
using UnityEngine;

[System.Serializable]  //A llows Unity to display it in the Inspector
public class HealthSystem
{
    // Fields (class variables)
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    // Constructor - sets initial values
    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    // Method: Take Damage
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log("Took damage: " + damageAmount + " | Health: " + currentHealth);
    }

    // Method: Heal
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("Healed: " + healAmount + " | Health: " + currentHealth);
    }

    // Method: Get current health
    public int GetHealth() => currentHealth;

    // Method: Is the object dead?
    public bool IsDead() => currentHealth <= 0;
}