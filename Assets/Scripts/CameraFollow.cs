using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform Target;

    [Header("Camera Position (Relative to Player)")]
    // X = Left/Right (0 = centered, 0.8 = over right shoulder), Y = Height, Z = Distance behind
    public Vector3 CameraOffset = new Vector3(0f, 2.5f, -4.5f);

    [Header("Focal Point (Where Camera Aims)")]
    // X = Aim left/right, Y = Aim height (1.2 = chest, 1.7 = head), Z = Aim forward ahead of player
    public Vector3 LookAtOffset = new Vector3(0f, 1.5f, 1.0f);

    [Header("Smoothing")]
    public float positionSmoothing = 8f;
    public float rotationSmoothing = 8f;

    [Header("Wall Collision")]
    public float cameraRadius = 0.25f;
    public float wallBuffer = 0.1f;
    public float minDistance = 0.5f;
    public LayerMask collisionLayers = ~0;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Target = playerObj.transform;
        }
    }

    private void LateUpdate()
    {
        if (Target == null) return;

        // 1. Where the camera wants to stand relative to the player
        Vector3 desiredPosition = Target.position + (Target.rotation * CameraOffset);

        // 2. Check for walls between the player's chest/head and desiredPosition
        Vector3 rayOrigin = Target.position + Vector3.up * LookAtOffset.y;
        Vector3 rayDirection = desiredPosition - rayOrigin;
        float maxDistance = rayDirection.magnitude;

        if (maxDistance > 0.001f && Physics.SphereCast(rayOrigin, cameraRadius, rayDirection.normalized, out RaycastHit hit, maxDistance, collisionLayers, QueryTriggerInteraction.Ignore))
        {
            float safeDistance = Mathf.Clamp(hit.distance - wallBuffer, minDistance, maxDistance);
            desiredPosition = rayOrigin + rayDirection.normalized * safeDistance;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothing * Time.deltaTime);

        // 3. The exact point the camera looks at (now adjustable in the Inspector)
        Vector3 lookTarget = Target.position + (Target.rotation * LookAtOffset);
        Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothing * Time.deltaTime);
    }
}