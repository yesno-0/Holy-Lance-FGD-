using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace NEDClass2024
{
    public class GameHUDManager : MonoBehaviour
    {
        public static GameHUDManager Instance;

        [Header("Health UI (Bottom-Left)")]
        public PlayerHealth playerHealth;
        public Slider healthSlider;
        public TMP_Text healthText;

        [Header("Key Counter UI (Top-Left)")]
        public TMP_Text keyCounterText;
        public int totalKeyParts = 4;

        [Header("Escape Timer UI (Top-Center)")]
        public TMP_Text escapeTimerText;
        public float escapeDuration = 25f;

        [Header("Interaction Prompts (Center)")]
        public GameObject pickupPromptUI;
        public GameObject doorPromptUI;

        private bool escapeActive = false;
        private float timeRemaining;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (playerHealth == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerHealth = player.GetComponent<PlayerHealth>();
            }

            if (healthSlider != null && playerHealth != null)
            {
                healthSlider.maxValue = playerHealth.maxHealth;
                healthSlider.value = playerHealth.currentHealth;
            }

            if (escapeTimerText != null) escapeTimerText.gameObject.SetActive(false);
            if (pickupPromptUI != null) pickupPromptUI.SetActive(false);
            if (doorPromptUI != null) doorPromptUI.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            UpdateHealthUI();
            UpdateKeyCounterUI();
            UpdateEscapeTimer();
        }

        private void UpdateHealthUI()
        {
            if (playerHealth == null) return;

            if (healthSlider != null)
            {
                healthSlider.maxValue = playerHealth.maxHealth;
                healthSlider.value = playerHealth.currentHealth;
            }

            if (healthText != null)
            {
                healthText.text = $"HP: {playerHealth.currentHealth} / {playerHealth.maxHealth}";
            }
        }

        private void UpdateKeyCounterUI()
        {
            if (keyCounterText != null)
            {
                keyCounterText.text = $"Key Parts: {KeyPiecePickup.PiecesCollected} / {totalKeyParts}";
            }
        }

        public void StartEscapeTimer()
        {
            escapeActive = true;
            timeRemaining = escapeDuration;

            if (escapeTimerText != null)
            {
                escapeTimerText.gameObject.SetActive(true);
            }
        }

        public void StopEscapeTimerWin()
        {
            escapeActive = false;
            if (escapeTimerText != null)
            {
                escapeTimerText.text = "ESCAPED WITH THE HOLY LANCE!";
            }
            Debug.Log("Mission Complete! Player escaped the bunker in time.");
        }

        private void UpdateEscapeTimer()
        {
            if (!escapeActive) return;

            timeRemaining -= Time.deltaTime;

            if (escapeTimerText != null)
            {
                escapeTimerText.text = $"ESCAPE BUNKER: {Mathf.Max(0f, timeRemaining):F1}s";
            }

            if (timeRemaining <= 0f)
            {
                escapeActive = false;
                Debug.Log("Time's up! Failed to escape the bunker.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        public void ShowPickupPrompt(bool show)
        {
            if (pickupPromptUI != null) pickupPromptUI.SetActive(show);
        }

        public void ShowDoorPrompt(bool show)
        {
            if (doorPromptUI != null) doorPromptUI.SetActive(show);
        }
    }
}