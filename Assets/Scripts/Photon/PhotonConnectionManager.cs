using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class PhotonConnectionManager : Singletons.PunSingletonInScene<PhotonConnectionManager>
    {
        [Header("Set Values")]
        [SerializeField] PhotonRoomManager roomManager;
        [SerializeField] string gameVersion = "1";
        [Header("Runtime Values")]
        [SerializeField] bool isConnecting;

        public System.Action Connecting;

        //public bool connecting { get { return isConnecting; } }

        //Unity Events
        private new void Awake()
        {
            base.Awake();

            //All clients auto-sync their scene with the photon version
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        void Start()
        {
            //If room manager doesn't exist, get it
            if (!roomManager)
            {
                roomManager = PhotonRoomManager.Get();

                //If room manager doesn't exist, exit before real errors appear
                if (!roomManager)
                {
                    Debug.LogError(gameObject + " room manager not connected");
                    Destroy(this);
                }
            }
        }

        //Methods
        public void Connect()
        {
            //Connect to room if connected to server, else, connect to server
            if (PhotonNetwork.IsConnected)
            {
                //Try to Join Random Room
                roomManager.JoinRandomRoom();
            }
            else
            {
                //Connect to server
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;

                //Call Actions
                Connecting?.Invoke();
            }
        }

        //Photon Events
        public override void OnConnectedToMaster()
        {
            //If player couldn't connect and function was called, don't join room
            if (!isConnecting) return;

            Debug.Log(gameObject.name + " Connected to Server");
            //After connecting to server, try to join random room
            roomManager.JoinRandomRoom();
            isConnecting = false;
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat(gameObject.name + " disconnected, {0}", cause);
            isConnecting = true;
        }
    }
}