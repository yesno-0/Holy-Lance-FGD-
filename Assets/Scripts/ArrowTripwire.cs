using UnityEngine;

public class ArrowTripwire : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public Transform aimTarget;
    public float fireCooldown = 2f;

    private float nextFireTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            FireArrow();
        }
    }

    private void FireArrow()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null) return;

        Vector3 shootDirection = (aimTarget != null)
            ? (aimTarget.position - arrowSpawnPoint.position).normalized
            : arrowSpawnPoint.forward;

        Quaternion shootRotation = Quaternion.LookRotation(shootDirection);
        GameObject arrowObj = Instantiate(arrowPrefab, arrowSpawnPoint.position, shootRotation);

        ArrowProjectile projectile = arrowObj.GetComponent<ArrowProjectile>();
        if (projectile != null)
        {
            projectile.Launch(shootDirection);
        }
    }
}