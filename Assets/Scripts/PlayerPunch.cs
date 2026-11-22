using UnityEngine;

namespace NEDClass2024
{
    public class PlayerPunch : MonoBehaviour
    {
        [Header("Combat Settings")]
        public int punchDamage = 20;
        public float punchRange = 2.2f;
        public float punchCooldown = 2f;

        [Header("References")]
        public Animator animator;

        private float lastPunchTime = -99f;

        private void Start()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= lastPunchTime + punchCooldown)
            {
                lastPunchTime = Time.time;
                PerformPunch();
            }
        }

        private void PerformPunch()
        {
            // Play the player's punch animation
            if (animator != null)
            {
                animator.SetTrigger("Punch");
            }

            // Detect enemy slightly in front of the player
            Vector3 hitCenter = transform.position + Vector3.up * 1f + transform.forward * 1f;
            Collider[] hits = Physics.OverlapSphere(hitCenter, punchRange * 0.6f);

            foreach (Collider hit in hits)
            {
                EnemyGuardAI enemy = hit.GetComponentInParent<EnemyGuardAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(punchDamage);
                    break;
                }
            }
        }
    }
}