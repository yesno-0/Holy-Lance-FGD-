using UnityEngine;

namespace NEDClass2024
{
    public class PlayerMovement : MonoBehaviour
    {
        public float Speed = 10f;
        public float RotationalSpeed = 120f;

        [Header("Jump & Gravity Settings")]
        public float JumpForce = 7f;
        public float Gravity = 20f;

        private CharacterController controller;
        private Animator animator;
        private float verticalVelocity = 0f;

        // Bridges SpikeTrap (and any legacy trap scripts) directly into PlayerHealth.cs
        public int PlayerHealth
        {
            get
            {
                PlayerHealth ph = GetComponent<PlayerHealth>();
                return ph != null ? ph.currentHealth : 0;
            }
            set
            {
                PlayerHealth ph = GetComponent<PlayerHealth>();
                if (ph != null)
                {
                    int damage = ph.currentHealth - value;
                    if (damage > 0)
                    {
                        ph.TakeDamage(damage);
                    }
                    else
                    {
                        ph.currentHealth = Mathf.Clamp(value, 0, ph.maxHealth);
                    }
                }
            }
        }

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Rotate player left/right
            transform.Rotate(0f, horizontal * RotationalSpeed * Time.deltaTime, 0f);

            // Stick cleanly to ground when walking down stairs/slopes
            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            // Trigger jump on Space only when grounded
            if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            {
                verticalVelocity = JumpForce;
                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                }
            }

            // Play Walking animation whenever any WASD / movement key is pressed
            bool isMoving = Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f;
            if (animator != null)
            {
                animator.SetBool("isWalking", isMoving && controller.isGrounded);
            }

            // Apply real gravity
            verticalVelocity -= Gravity * Time.deltaTime;

            // Move via CharacterController
            Vector3 move = (transform.forward * vertical * Speed) + (Vector3.up * verticalVelocity);
            controller.Move(move * Time.deltaTime);
        }
    }
}