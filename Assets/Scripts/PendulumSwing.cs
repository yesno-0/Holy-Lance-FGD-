using UnityEngine;

public class PendulumSwing : MonoBehaviour
{
    [Header("Swing Settings")]
    public float maxAngle = 35f;   // Lowered to 35 so it stops before hitting your bunker walls
    public float speed = 2.5f;
    public float timeOffset = 0f;

    // Optional: Check this in the Inspector if it swings down the hallway instead of across it
    public bool swingOnXAxis = false;

    private Quaternion startRotation;

    void Start()
    {
        // Automatically grabs whatever rotation you set on AxePivot in the Scene view
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = maxAngle * Mathf.Sin((Time.time + timeOffset) * speed);

        // Applies the swing relative to AxePivot's starting bunker angle
        Quaternion swingOffset = swingOnXAxis
            ? Quaternion.Euler(angle, 0, 0)
            : Quaternion.Euler(0, 0, angle);

        transform.localRotation = startRotation * swingOffset;
    }
}