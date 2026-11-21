using System.Collections;
using UnityEngine;
using NEDClass2024;

public class SpikeTrap : MonoBehaviour
{
    [Header("Spike Settings")]
    public Transform spikesMesh;
    public float riseHeight = 0.85f;
    public float riseSpeed = 35f;
    public float stayUpDuration = 1.5f;
    public int damageAmount = 15;

    private Vector3 peekingPosition;
    private Vector3 extendedPosition;
    private bool isTriggered = false;

    private void Start()
    {
        if (spikesMesh != null)
        {
            peekingPosition = spikesMesh.localPosition;
            extendedPosition = peekingPosition + Vector3.up * riseHeight;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
            if (player == null) player = other.GetComponentInChildren<PlayerMovement>();
            if (player == null) player = FindObjectOfType<PlayerMovement>();

            if (player != null)
            {
                player.PlayerHealth -= damageAmount;
                Debug.Log("Spike hit! Remaining HP: " + player.PlayerHealth);
            }

            StartCoroutine(AnimateSpikes());
        }
    }

    private IEnumerator AnimateSpikes()
    {
        isTriggered = true;

        while (spikesMesh != null && Vector3.Distance(spikesMesh.localPosition, extendedPosition) > 0.01f)
        {
            spikesMesh.localPosition = Vector3.MoveTowards(spikesMesh.localPosition, extendedPosition, riseSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(stayUpDuration);

        while (spikesMesh != null && Vector3.Distance(spikesMesh.localPosition, peekingPosition) > 0.01f)
        {
            spikesMesh.localPosition = Vector3.MoveTowards(spikesMesh.localPosition, peekingPosition, (riseSpeed * 0.4f) * Time.deltaTime);
            yield return null;
        }

        isTriggered = false;
    }
}