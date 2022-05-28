using UnityEngine;
using UnityEngine.InputSystem;

namespace MultiplayerGame.Gameplay.Player
{
    public class InputManager : Universal.Singletons.MonoBehaviourSingletonInScene<InputManager>
    {
        [Header("Set Values")]
        public PlayerController player;

        //Unity Events
        public void OnShootInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            player.OnShootInput(context);
        }
        public void OnMouseInput(InputAction.CallbackContext context)
        {
            player.OnMouseInput(context);
        }
    }
}
