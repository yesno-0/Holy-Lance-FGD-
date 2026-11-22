using UnityEngine;

namespace NEDClass2024
{
    public class BunkerEntranceDoor : MonoBehaviour
    {
        [Header("Double Door Hinges")]
        public Transform leftHinge;
        public Transform rightHinge;

        [Header("Swing Settings")]
        public float leftOpenAngle = -90f;
        public float rightOpenAngle = 90f;
        public float swingSpeed = 5f;

        private bool isOpen = false;
        private bool isLockedShut = false;
        private bool playerNearOutside = false;

        private Quaternion leftClosedRot;
        private Quaternion leftOpenRot;
        private Quaternion rightClosedRot;
        private Quaternion rightOpenRot;

        private void Start()
        {
            if (leftHinge != null)
            {
                leftClosedRot = leftHinge.localRotation;
                leftOpenRot = leftClosedRot * Quaternion.Euler(0f, leftOpenAngle, 0f);
            }

            if (rightHinge != null)
            {
                rightClosedRot = rightHinge.localRotation;
                rightOpenRot = rightClosedRot * Quaternion.Euler(0f, rightOpenAngle, 0f);
            }
        }

        private void Update()
        {
            if (!isLockedShut && !isOpen && playerNearOutside && Input.GetKeyDown(KeyCode.E))
            {
                isOpen = true;
                if (GameHUDManager.Instance != null)
                {
                    GameHUDManager.Instance.ShowDoorPrompt(false);
                }
            }

            if (leftHinge != null)
            {
                Quaternion targetLeft = isOpen ? leftOpenRot : leftClosedRot;
                leftHinge.localRotation = Quaternion.Slerp(leftHinge.localRotation, targetLeft, swingSpeed * Time.deltaTime);
            }

            if (rightHinge != null)
            {
                Quaternion targetRight = isOpen ? rightOpenRot : rightClosedRot;
                rightHinge.localRotation = Quaternion.Slerp(rightHinge.localRotation, targetRight, swingSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isLockedShut && !isOpen)
            {
                playerNearOutside = true;
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
                playerNearOutside = false;
                if (GameHUDManager.Instance != null)
                {
                    GameHUDManager.Instance.ShowDoorPrompt(false);
                }
            }
        }

        public void LockDoorBehindPlayer()
        {
            if (!isLockedShut)
            {
                isOpen = false;
                isLockedShut = true;
                if (GameHUDManager.Instance != null)
                {
                    GameHUDManager.Instance.ShowDoorPrompt(false);
                }
                Debug.Log("[Door Debug] Bunker entrance sealed!");
            }
        }
    }
}