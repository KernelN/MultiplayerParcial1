using MultiplayerGame.Gameplay;
using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class UIGameplayManagerPhoton : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] GameplayManagerPhoton manager;
        [SerializeField] UIGameplayManager offlineUI;

        //Unity Events
        private void Start()
        {
            if (!manager)
            {
                manager = GameplayManagerPhoton.Get();
            }

            HijackUI();
        }

        //Methods
        void HijackUI()
        {
            manager.GameOver += offlineUI.OnGameOver;
        }
    }
}
