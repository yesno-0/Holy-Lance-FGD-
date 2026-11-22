using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NEDClass2024
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        public int maxHealth = 100;
        public int currentHealth;
        public float gameOverRestartDelay = 1.5f;

        private bool isDead = false;

        private void Start()
        {
            currentHealth = maxHealth;
            SyncUI();
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            Debug.Log($"Player took {damage} damage! Health: {currentHealth}/{maxHealth}");
            SyncUI();

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public void RestoreFullHealth()
        {
            if (isDead) return;

            currentHealth = maxHealth;
            Debug.Log($"Enemy defeated! Player health restored to {currentHealth}/{maxHealth}.");
            SyncUI();
        }

        private void HandleDeath()
        {
            isDead = true;
            currentHealth = 0;
            SyncUI();
            Debug.Log("You died! Resetting all progress...");
            StartCoroutine(RestartLevelCoroutine());
        }

        private IEnumerator RestartLevelCoroutine()
        {
            yield return new WaitForSeconds(gameOverRestartDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void SyncUI()
        {
            if (GameHUDManager.Instance != null)
            {
                if (GameHUDManager.Instance.healthSlider != null)
                {
                    GameHUDManager.Instance.healthSlider.maxValue = maxHealth;
                    GameHUDManager.Instance.healthSlider.value = currentHealth;
                }

                if (GameHUDManager.Instance.healthText != null)
                {
                    GameHUDManager.Instance.healthText.text = $"{currentHealth} / {maxHealth}";
                }
            }
        }
    }
}