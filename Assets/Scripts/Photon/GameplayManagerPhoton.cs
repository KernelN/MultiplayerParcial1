using MultiplayerGame.Photon.Singletons;
using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class GameplayManagerPhoton : PunSingletonInScene<GameplayManagerPhoton>
    {
        [Header("Set Values")]
        [SerializeField] Gameplay.GameplayManager manager;
        [SerializeField] PhotonRoomManager roomManager;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] Transform spawnCenter;
        [SerializeField] Vector2 spawnRadius;
        [Header("Runtime Values")]
        [SerializeField] int maxPlayers;
        [SerializeField] int playerNumber;
        [SerializeField] bool victory;

        public System.Action<bool> GameOver;

        //Unity Events
        private void Start()
        {
            //Get Gameplay Manager
            if (!manager)
            {
                manager = Gameplay.GameplayManager.Get();
            }

            //Get Room Manager
            if (!roomManager)
            {
                roomManager = PhotonRoomManager.Get();
            }

            //Get player numbers (of room)
            maxPlayers = roomManager.publicMaxUsers;
            playerNumber = PhotonNetwork.LocalPlayer.ActorNumber; //roomManager.publicUserNumber;

            //Create player
            InstantiatePlayer();

            if (PhotonNetwork.IsMasterClient)
            {
                manager.GameOver += OnGameOver;
            }
            else
            {
                Destroy(manager);
            }
        }

        //Methods
        public void ExitRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        void InstantiatePlayer()
        {
            //Get Instance Values
            string prefab = playerPrefab.name;
            Vector2 pos = GetPlayerPos();
            Quaternion rot = transform.rotation;

            //Instantiate (online)
            GameObject player = PhotonNetwork.Instantiate(prefab, pos, rot);
        }
        Vector2 GetPlayerPos()
        {
            //Get how many players will be per row (row x column = players)
            float playerPerRow = Mathf.Sqrt(maxPlayers);

            //Calculate distances
            float xDistance = (spawnRadius.x) / playerPerRow;
            float yDistance = (spawnRadius.y) / playerPerRow;

            //Set First position
            float xPos = transform.position.x - spawnRadius.x;
            float yPos = transform.position.y - spawnRadius.y;

            //Move equally to player number
            xPos += xDistance * playerNumber;

            //Fix position until is in area
            while (xPos > spawnRadius.x)
            {
                xPos -= spawnRadius.x;
                yPos += yDistance;
            }

            return new Vector2(xPos, yPos);
        }

        //Event Receivers
        void OnGameOver(bool victory)
        {
            //Call Event
            GameOver?.Invoke(victory);

            //If player is host, send victory message to others
            if (!PhotonNetwork.IsMasterClient) return;
            photonView.RPC("OnGameOver", RpcTarget.Others, victory);
        }
    }
}
