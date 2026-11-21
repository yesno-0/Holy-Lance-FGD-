using UnityEngine;
using NEDClass2024; // Lets this script see your PlayerMovement class

public class TrapDamage : MonoBehaviour
{
    public int damage = 25;
    public float hitCooldown = 1f;
    private float lastHitTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= lastHitTime + hitCooldown)
        {
            lastHitTime = Time.time;

            // Grabs your PlayerMovement script and subtracts from the PlayerHealth variable
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.PlayerHealth -= damage;
                Debug.Log("Trap hit! Remaining HP: " + player.PlayerHealth);
            }
        }
    }
}