using MultiplayerGame.Gameplay.Player;
using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class ProjectilePhoton : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] ProjectileController controller;
        [SerializeField] PhotonView punView;
        [SerializeField] Transform projectilesEmpty;

        //Unity Events
        private void Start()
        {
            if (!controller)
            {
                controller = GetComponent<ProjectileController>();
            }
            if (!projectilesEmpty)
            {
                projectilesEmpty = GameObject.FindGameObjectWithTag("Projectiles").transform;
            }
            transform.parent = projectilesEmpty;

            controller.Destroyed += OnDestroyed;
        }

        //Event Receivers
        void OnDestroyed()
        {
            PhotonNetwork.Destroy(punView);
        }
    }
}
