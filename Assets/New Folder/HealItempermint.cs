using UnityEngine;

public class Healitempermint : MonoBehaviour
{
    [SerializeField] private int healAmount = 25; // how much to heal

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log($"Healed player by {healAmount}!");

            
        }
    }
}