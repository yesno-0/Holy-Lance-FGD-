using UnityEngine;
using NEDClass2024;

public class BoulderTrap : MonoBehaviour
{
    public GameObject boulderPrefab;
    public Transform startPoint;
    public float launchPush = 12f;
    public int damageAmount = 30;
    public float cooldown = 3f;

    private float nextSpawnTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + cooldown;

            GameObject boulder = Instantiate(boulderPrefab, startPoint.position, startPoint.rotation);

            Rigidbody rb = boulder.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = startPoint.forward * launchPush;
            }

            BoulderCollision hit = boulder.AddComponent<BoulderCollision>();
            hit.damage = damageAmount;
        }
    }
}

public class BoulderCollision : MonoBehaviour
{
    public int damage = 30;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.down * 30f, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponentInParent<PlayerMovement>();
            if (player == null) player = FindObjectOfType<PlayerMovement>();

            if (player != null)
            {
                player.PlayerHealth -= damage;
                Debug.Log("Hit by boulder! Remaining HP: " + player.PlayerHealth);
            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}