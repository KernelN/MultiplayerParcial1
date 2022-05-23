using System;
using UnityEngine;
using Universal.Singletons;

namespace MultiplayerGame.Gameplay
{
    public class GameplayManager : MonoBehaviourSingletonInScene<GameplayManager>
    {
        [Header("Set Values")]
        [SerializeField] AreaController[] winAreas;
        [SerializeField] float maxGameTime;
        [Header("Runtime Values")]
        [SerializeField] float timer;
        [SerializeField] int activeAreas;

        public Action<bool> GameOver; //Game Over, player won? > (true/false)

        //Unity Events
        private void Start()
        {
            foreach (var area in winAreas)
            {
                area.PoolMaxed += OnAreaActivated;
            }
        }
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= maxGameTime)
            {
                GameOver?.Invoke(false);
            }
        }

        //Methods

        //Event Receiver
        void OnAreaActivated(bool areaActivated)
        {
            activeAreas += areaActivated ? 1 : -1;
            
            if (activeAreas >= winAreas.Length)
            {
                GameOver?.Invoke(true);
            }
        }
    }
}
