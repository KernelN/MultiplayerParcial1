using UnityEngine;

namespace MultiplayerGame.Gameplay
{
    public class CameraFollower : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] Transform playerController;

        //Unity Events
        private void FixedUpdate()
        {
            if (!playerController) return;
            transform.position = playerController.position;
        }
    }
}
