using UnityEngine;
using NEDClass2024;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 15;
    public float lifeTime = 4f;
    public Vector3 meshAxisFix = Vector3.zero;

    private Vector3 moveDirection;
    private bool isLaunched = false;

    public void Launch(Vector3 direction)
    {
        moveDirection = direction.normalized;
        isLaunched = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true; // Prevents physics deflection off walls/dispensers
        }

        transform.rotation = Quaternion.LookRotation(moveDirection) * Quaternion.Euler(meshAxisFix);
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (isLaunched)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
            if (player == null) player = other.GetComponentInChildren<PlayerMovement>();
            if (player == null) player = FindObjectOfType<PlayerMovement>();

            if (player != null)
            {
                player.PlayerHealth -= damage;
                Debug.Log("Arrow hit! Remaining HP: " + player.PlayerHealth);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            // Only destroy when hitting the opposite wall, not the dispenser it spawned inside
            Destroy(gameObject);
        }
    }
}