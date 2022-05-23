using UnityEngine;
using UnityEngine.InputSystem;

namespace MultiplayerGame.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform projectilesEmpty;
        [SerializeField] float shootSpawnRadius;
        [SerializeField] int maxProjectiles;
        [Header("Runtime Values")]
        [SerializeField] Vector2 mousePos;
        [SerializeField] int currentProjectiles;

        //Unity Events
        private void Start()
        {
            //If projectile would spawn inside player, increase radius
            if (shootSpawnRadius < transform.lossyScale.x / 2)
            {
                shootSpawnRadius = transform.lossyScale.x * 0.6f;
            }
        }
        public void OnShootInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            ShootProjectile();
        }
        public void OnMouseInput(InputAction.CallbackContext context)
        {
            //Get Input
            mousePos = context.ReadValue<Vector2>();

            //Convert to game pos
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        }

        //Methods
        void ShootProjectile()
        {
            //If max projectiles reached, exit
            if (currentProjectiles >= maxProjectiles) return;
            currentProjectiles++;
            
            //Get Direction of shooting
            Vector2 shootDirection = mousePos - (Vector2)transform.position;

            //Spawn Projectile
            GameObject projectile = Instantiate(projectilePrefab, projectilesEmpty);
            Vector2 spawnPos = (Vector2)transform.position + shootDirection.normalized * shootSpawnRadius;
            projectile.transform.position = spawnPos;
            projectile.name = gameObject.name + "'s Disc " + currentProjectiles;

            //Set Controller Values
            ProjectileController projController = projectile.GetComponent<ProjectileController>();
            projController.movement = shootDirection;
            projController.shooter = transform;

            //Link Actions
            projController.Destroyed += OnProjectileDestroyed;
        }

        //Event Receivers
        void OnProjectileDestroyed()
        {
            currentProjectiles--;
            if (currentProjectiles < 0)
            {
                currentProjectiles = 0;
            }
        }
    }
}
