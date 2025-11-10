using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private int healAmount = 25; // how much to heal

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log($"Healed player by {healAmount}!");

            // Destroy the pickup after use
            Destroy(gameObject);
        }
    }
}