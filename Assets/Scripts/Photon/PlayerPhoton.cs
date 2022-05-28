using MultiplayerGame.Gameplay.Player;
using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class PlayerPhoton : MonoBehaviourPunCallbacks
    {
        [Header("Set Values")]
        [SerializeField] PlayerController controller;
        [SerializeField] GameObject mainCamera;
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform playersEmpty;
        [SerializeField] SpriteRenderer mesh;
        [SerializeField] float shootSpawnRadius;

        //Unity Events
        private void Start()
        {
            //Get Controller
            if (!controller)
            {
                controller = GetComponent<PlayerController>();
            }

            //Get Empty an set as parent
            if (!playersEmpty)
            {
                playersEmpty = GameObject.FindGameObjectWithTag("Players").transform;
            }
            transform.parent = playersEmpty;
            
            //If is host don't use the playable prefab
            if (!photonView.IsMine)
            {
                Destroy(controller);
                Destroy(mainCamera);
            }
            else
            {
                //Set Player Controls
                InputManager inputs = InputManager.Get();
                if (inputs)
                {
                    inputs.player = controller;
                }

                //Link Actions
                controller.ProjectileShot += OnProjectileShot;

                //If projectile would spawn inside player, increase radius
                if (shootSpawnRadius < transform.lossyScale.x / 2)
                {
                    shootSpawnRadius = transform.lossyScale.x * 0.6f;
                }                
            }
        }

        //Methods
        void LaunchProjectile(Vector2 mousePos)
        {
            //Get Direction of shooting
            Vector2 shootDirection = mousePos - (Vector2)transform.position;

            //Spawn Projectile
            Vector2 spawnPos = (Vector2)transform.position + shootDirection.normalized * shootSpawnRadius;
            GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, spawnPos, transform.rotation);
            projectile.name = gameObject.name + "'s Disc " + controller.publicProjectiles;

            //Set Controller Values
            ProjectileController projController = projectile.GetComponent<ProjectileController>();
            projController.movement = shootDirection;
            projController.shooter = transform;

            //Link Actions
            projController.Destroyed += controller.OnProjectileDestroyed;
        }

        //Event Recievers
        void OnProjectileShot(Vector2 mousePos)
        {
            LaunchProjectile(mousePos);
        }
    }
}