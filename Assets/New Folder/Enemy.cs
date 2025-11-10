using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    public int startingHealth = 50;

    public int contactDamage = 10;

    private float lastHitTime = 0f;
    public float hitCooldown = 1f;

    [SerializeField] private HealthBar healthBar;  // 👈 Reference to health bar UI


    void Start()
    {
        // Enemy starts with less health
        healthSystem = new HealthSystem(startingHealth);

        healthBar.SetMaxHealth(startingHealth);

    }

    public void AttackPlayer(Player player)
    {
        Debug.Log("Enemy attacks!");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - lastHitTime < hitCooldown) return;

        // If player touches the enemy, the player takes damage
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Enemy touched Player!");
            player.TakeHit(contactDamage);
            lastHitTime = Time.time;
        }
    }


    public void TakeHit(int damage)
    {
        healthSystem.TakeDamage(damage);
        healthBar.SetHealth(healthSystem.GetHealth());


        if (healthSystem.IsDead())
        {
            Debug.Log("Enemy defeated!");
            Destroy(gameObject); // Remove enemy from scene

        }
    }
}
