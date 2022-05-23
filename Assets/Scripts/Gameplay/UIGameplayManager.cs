using UnityEngine;
using Universal.Singletons;

namespace MultiplayerGame.Gameplay
{
    public class UIGameplayManager : MonoBehaviourSingletonInScene<UIGameplayManager>
    {
        [Header("Set Values")]
        [SerializeField] GameplayManager manager;
        [SerializeField] GameObject loseScreen;
        [SerializeField] GameObject winScreen;

        //Unity Events
        private void Start()
        {
            if (!manager)
            {
                manager = GameplayManager.Get();
            }

            manager.GameOver += OnGameOver;
        }

        //Methods

        //Event Receivers
        void OnGameOver(bool playersWon)
        {
            loseScreen.SetActive(!playersWon);
            winScreen.SetActive(playersWon);
        }
    }
}
