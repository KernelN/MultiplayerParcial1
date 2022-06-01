using UnityEngine;

namespace MultiplayerGame.Gameplay
{
    public class CameraFollower : MonoBehaviour
    {
        [Header("Set Values")]
        public Transform playerController;

        //Unity Events
        private void FixedUpdate()
        {
            if (!playerController) return;
            Vector3 newPos = playerController.position;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }
}
