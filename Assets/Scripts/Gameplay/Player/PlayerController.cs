using UnityEngine;
using UnityEngine.InputSystem;

namespace MultiplayerGame.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] int maxProjectiles;
        [Header("Runtime Values")]
        [SerializeField] Vector2 mousePos;
        [SerializeField] int currentProjectiles;

        public System.Action<Vector2> ProjectileShot;

        public int publicProjectiles {  get { return currentProjectiles; } }

        //Unity Events
        private void Start()
        {
            mousePos = Vector2.zero;
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

            //Call action
            ProjectileShot?.Invoke(mousePos);
        }

        //Event Receivers
        public void OnProjectileDestroyed()
        {
            currentProjectiles--;
            if (currentProjectiles < 0)
            {
                currentProjectiles = 0;
            }
        }
    }
}