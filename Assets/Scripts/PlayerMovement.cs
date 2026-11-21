using UnityEngine;

namespace NEDClass2024
{
    public class PlayerMovement : MonoBehaviour
    {
        public float Speed = 10f;
        public float RotationalSpeed = 120f;
        public int PlayerHealth = 100;

        [Header("Jump & Gravity Settings")]
        public float JumpForce = 7f;
        public float Gravity = 20f;

        private CharacterController controller;
        private float verticalVelocity = 0f;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
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

            // Trigger jump on Space
            if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            {
                verticalVelocity = JumpForce;
            }

            // Apply real gravity
            verticalVelocity -= Gravity * Time.deltaTime;

            // Combine forward movement + gravity/jump and let CharacterController handle walls & stairs
            Vector3 move = (transform.forward * vertical * Speed) + (Vector3.up * verticalVelocity);
            controller.Move(move * Time.deltaTime);
        }
    }
}