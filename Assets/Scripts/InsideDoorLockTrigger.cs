using UnityEngine;

namespace NEDClass2024
{
    public class InsideDoorLockTrigger : MonoBehaviour
    {
        public BunkerEntranceDoor entranceDoor;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && entranceDoor != null)
            {
                entranceDoor.LockDoorBehindPlayer();
            }
        }
    }
}