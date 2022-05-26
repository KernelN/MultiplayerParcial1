using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class UIPhotonConnectionManager : MonoBehaviourPunCallbacks
    {
        [Header("Set Values")]
        [SerializeField] PhotonConnectionManager manager;
        [SerializeField] GameObject inputPanel;
        [SerializeField] GameObject connectingText;

        //Unity Events
        private void Start()
        {
            if (!manager)
            {
                manager = PhotonConnectionManager.Get();
            }

            manager.Connecting += OnConnected;

            inputPanel.SetActive(true);
            connectingText.SetActive(false);
        }


        //Event Receivers
        void OnConnecting()
        {
            inputPanel.SetActive(false);
            connectingText.SetActive(true);
        }
    }
}