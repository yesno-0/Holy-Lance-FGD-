using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform Target;

    // Set this in the Inspector (e.g., X: 0, Y: 3, Z: -5)
    public Vector3 CameraOffset = new Vector3(0, 3, -5);

    public float positionSmoothing = 5f;
    public float rotationSmoothing = 5f;

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

        // Places the camera behind the player's current facing direction
        Vector3 desiredPosition = Target.position + (Target.rotation * CameraOffset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothing * Time.deltaTime);

        // Smoothly rotates the camera to look where the player is looking
        Quaternion desiredRotation = Quaternion.LookRotation(Target.position + Vector3.up * 1.2f - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothing * Time.deltaTime);
    }
}