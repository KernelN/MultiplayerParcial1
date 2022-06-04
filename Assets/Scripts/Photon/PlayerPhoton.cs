using MultiplayerGame.Gameplay;
using MultiplayerGame.Gameplay.Player;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace MultiplayerGame.Photon
{
    public class PlayerPhoton : MonoBehaviourPunCallbacks
    {
        [Header("Set Values")]
        [SerializeField] PhotonRoomManager roomManager;
        [SerializeField] PlayerController controller;
        [SerializeField] CameraFollower mainCamera;
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform playersEmpty;
        [SerializeField] SpriteRenderer mesh;
        [SerializeField] Light2D light2d;
        [SerializeField] TrailRenderer trail;
        [SerializeField] float shootSpawnRadius;
        [Header("Runtime Values")]
        [SerializeField] int totalPlayers;
        [SerializeField] int playerNumber;

        //Unity Events
        private void Start()
        {
            //Get Room Manager
            if (!roomManager)
            {
                roomManager = PhotonRoomManager.Get();
            }

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

            //Get player numbers
            totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
            playerNumber = photonView.OwnerActorNr;

            //Set name and color
            SetColor(GetComponent<SpriteRenderer>());
            string nick = PhotonNetwork.CurrentRoom.GetPlayer(photonView.OwnerActorNr).NickName;
            gameObject.name = nick;

            //If is host don't use the playable prefab
            if (!photonView.IsMine)
            {
                Destroy(controller);
                return;
            }

            //Set Player Controls
            InputManager inputs = InputManager.Get();
            if (inputs)
            {
                inputs.player = controller;
            }

            //Get Camera
            mainCamera = Camera.main.gameObject.GetComponent<CameraFollower>();
            mainCamera.playerController = transform;

            //Link Actions
            controller.ProjectileShot += OnProjectileShot;

            //If projectile would spawn inside player, increase radius
            if (shootSpawnRadius < transform.lossyScale.x / 2)
            {
                shootSpawnRadius = transform.lossyScale.x * 0.6f;
            }
        }

        //Methods
        void SetColor(SpriteRenderer renderer)
        {
            //If it's first or last player, set fast color and exit
            if (playerNumber == totalPlayers)
            {
                renderer.material.color = Color.black;
                light2d.color = Color.black;
                trail.startColor = Color.black;
                return;
            }
            else if (playerNumber == 1)
            {
                renderer.material.color = Color.white;
                light2d.color = Color.white;
                trail.startColor = Color.white;
                return;
            }

            //Get Original Color
            Color newColor = renderer.material.color;

            //Get player position in total and assing value
            float colorDistance = 1 / ((float)totalPlayers);
            float colorPool = colorDistance * playerNumber;
            float x = colorPool * Mathf.PI;

            //Get color with value
            newColor.r = Mathf.Sin(colorPool * 8);
            newColor.g = Mathf.Cos(colorPool * 10 + 1.8f);
            if (colorPool > 0.3f)
                newColor.b = Mathf.Cos(colorPool * 10 - 0.5f);

            //Update Color
            renderer.material.color = newColor;
            light2d.color = newColor;
            trail.startColor = newColor;
        }
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
            Debug.Log("Shoot with direction " + mousePos);
            LaunchProjectile(mousePos);
        }
    }
}