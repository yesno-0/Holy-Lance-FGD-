using UnityEngine;

namespace NEDClass2024
{
    public class FinalChamberGate : MonoBehaviour
    {
        [Header("Gate Settings")]
        public int requiredPieces = 4;
        public Transform doorMesh;
        public float openAngle = 90f;
        public float openSpeed = 3f;

        private bool playerInRange = false;
        private bool isUnlocked = false;
        private Quaternion closedRotation;
        private Quaternion targetRotation;

        private void Start()
        {
            if (doorMesh == null) doorMesh = transform;

            closedRotation = doorMesh.localRotation;
            targetRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
        }

        private void Update()
        {
            if (playerInRange && !isUnlocked && Input.GetKeyDown(KeyCode.E))
            {
                if (KeyPiecePickup.PiecesCollected >= requiredPieces)
                {
                    isUnlocked = true;
                    Debug.Log("Final Chamber Gate Unlocked!");

                    if (GameHUDManager.Instance != null)
                    {
                        GameHUDManager.Instance.ShowDoorPrompt(false);
                    }
                }
                else
                {
                    Debug.Log($"Locked! You have {KeyPiecePickup.PiecesCollected}/{requiredPieces} key pieces.");
                }
            }

            if (isUnlocked)
            {
                doorMesh.localRotation = Quaternion.Slerp(doorMesh.localRotation, targetRotation, openSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isUnlocked)
            {
                playerInRange = true;
                if (GameHUDManager.Instance != null)
                {
                    GameHUDManager.Instance.ShowDoorPrompt(true);
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
                    GameHUDManager.Instance.ShowDoorPrompt(false);
                }
            }
        }
    }
}