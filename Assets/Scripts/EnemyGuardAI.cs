using UnityEngine;

namespace NEDClass2024
{
    public class EnemyGuardAI : MonoBehaviour
    {
        [Header("Health & Combat")]
        public int maxHealth = 60;
        public int punchDamage = 15;
        public float attackRange = 2.5f;
        public float attackCooldown = 1.4f;

        [Header("Detection & Chase")]
        public float detectionRange = 10f;
        public float moveSpeed = 3.5f;

        [Header("References")]
        public Transform player;
        public Animator animator;

        private int currentHealth;
        private float lastAttackTime = -99f;
        private bool isDead = false;

        private void Start()
        {
            currentHealth = maxHealth;

            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }

            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) player = playerObj.transform;
            }
        }

        private void Update()
        {
            if (isDead || player == null) return;

            Vector3 flatEnemyPos = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 flatPlayerPos = new Vector3(player.position.x, 0f, player.position.z);
            float distanceToPlayer = Vector3.Distance(flatEnemyPos, flatPlayerPos);

            // 1. Out of sight -> stand guard
            if (distanceToPlayer > detectionRange)
            {
                SetRunning(false);
                return;
            }

            // Face the player smoothly
            Vector3 lookDir = flatPlayerPos - flatEnemyPos;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 10f * Time.deltaTime);
            }

            // 2. Check if player is inside punch sphere around enemy's chest
            Vector3 punchOrigin = transform.position + Vector3.up * 1f;
            bool inPunchRange = distanceToPlayer <= attackRange || IsPlayerInSphere(punchOrigin, attackRange);

            if (inPunchRange)
            {
                SetRunning(false);

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;
                    PunchPlayer();
                }
            }
            else
            {
                SetRunning(true);
                Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }
        }

        private bool IsPlayerInSphere(Vector3 center, float radius)
        {
            Collider[] hits = Physics.OverlapSphere(center, radius);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].CompareTag("Player") || hits[i].transform.root == player.root)
                {
                    return true;
                }
            }
            return false;
        }

        private void PunchPlayer()
        {
            if (animator != null)
            {
                animator.SetTrigger("Punch");
            }

            // Directly find PlayerHealth anywhere on the player hierarchy so SendMessage can't miss
            PlayerHealth health = player.GetComponentInParent<PlayerHealth>();
            if (health == null) health = player.GetComponentInChildren<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(punchDamage);
            }
            else
            {
                Debug.LogWarning("Enemy punched, but no PlayerHealth script was found on Player!");
            }
        }

        public void TakeDamage(int amount)
        {
            if (isDead) return;

            currentHealth -= amount;
            Debug.Log($"Enemy guard took {amount} damage! HP: {Mathf.Max(0, currentHealth)}/{maxHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            SetRunning(false);

            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            if (player != null)
            {
                PlayerHealth health = player.GetComponentInParent<PlayerHealth>();
                if (health == null) health = player.GetComponentInChildren<PlayerHealth>();
                if (health != null) health.RestoreFullHealth();
            }

            Debug.Log("Enemy guard defeated! Player health restored to full.");
            Destroy(gameObject, 3f);
        }

        private void SetRunning(bool running)
        {
            if (animator != null)
            {
                animator.SetBool("IsRunning", running);
            }
        }
    }
}