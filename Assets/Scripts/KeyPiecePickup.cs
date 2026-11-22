using UnityEngine;

namespace NEDClass2024
{
    public class KeyPiecePickup : MonoBehaviour
    {
        public static int PiecesCollected = 0;

        private bool playerInRange = false;

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                PiecesCollected++;
                Debug.Log($"Collected Key Piece! Total: {PiecesCollected}/4");

                if (GameHUDManager.Instance != null)
                {
                    GameHUDManager.Instance.ShowPickupPrompt(false);
                }

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