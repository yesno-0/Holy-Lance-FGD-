using UnityEngine;

namespace NEDClass2024
{
    public class HolyLancePickup : MonoBehaviour
    {
        private bool playerInRange = false;

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                if (GameHUDManager.Instance != null)
                {
                    GameHUDManager.Instance.ShowPickupPrompt(false);
                    GameHUDManager.Instance.StartEscapeTimer();
                }

                Debug.Log("Holy Lance stolen! 50 seconds to escape the bunker!");
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                if (GameHUDManager.Instance != null)
                {
                    GameHUDManager.Instance.ShowPickupPrompt(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                if (GameHUDManager.Instance != null)
                {
                    GameHUDManager.Instance.ShowPickupPrompt(false);
                }
            }
        }
    }
}