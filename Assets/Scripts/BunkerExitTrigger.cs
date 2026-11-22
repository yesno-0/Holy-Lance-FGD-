using UnityEngine;

namespace NEDClass2024
{
    public class BunkerExitTrigger : MonoBehaviour
    {
        private bool hasEscaped = false;

        private void OnTriggerEnter(Collider other)
        {
            if (hasEscaped) return;

            if (other.CompareTag("Player") || (other.transform.root != null && other.transform.root.CompareTag("Player")))
            {
                if (GameHUDManager.Instance != null && GameHUDManager.Instance.escapeTimerText != null && GameHUDManager.Instance.escapeTimerText.gameObject.activeSelf)
                {
                    hasEscaped = true;
                    GameHUDManager.Instance.StopEscapeTimerWin();
                }
            }
        }
    }
}