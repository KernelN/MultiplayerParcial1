using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Photon.Singletons
{
    public class PunSingleton<T> : MonoBehaviourPunCallbacks where T : Component
    {
        private static T instance;

        public static T Get()
        {
            return instance;
        }

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}