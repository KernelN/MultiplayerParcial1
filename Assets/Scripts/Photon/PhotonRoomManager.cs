using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class PhotonRoomManager : Singletons.PunSingletonInScene<PhotonRoomManager>
    {
        [Header("Set Values")]
        [SerializeField] [Range(1, 20)] byte maxPlayersPerRoom = 4;
        [Header("Runtime Values")]
        [SerializeField] List<Player> users;
        [SerializeField] int userNumber;

        public List<Player> publicUsers { get { return users; } }
        public int publicUserNumber { get { return userNumber; } }
        public int publicMaxUsers { get { return maxPlayersPerRoom; } }

        //Unity Events
        private void Start()
        {
            users = new List<Player>();
        }

        //Methods
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        void LoadGameRoom()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Debug.Log("Loading Room Lobby");
            PhotonNetwork.LoadLevel("Room");
        }
        public void LoadLevel()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Debug.Log("Loading Game");
            PhotonNetwork.LoadLevel("GameplayHost");
        }

        //Photon Events (all this ARE techincally, TCP)
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(gameObject + " No random room available, create one");

            //No random room available, create one
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("You joined room");
            userNumber = PhotonNetwork.CurrentRoom.PlayerCount;
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                users.Add(player.Value);
            }
            LoadGameRoom();
        }
        public override void OnLeftRoom()
        {
            users.Clear();
            GameManager.Get().LoadScene(Universal.SceneManaging.Scenes.lobby);
        }
        public override void OnPlayerEnteredRoom(Player other)
        {
            //Doesn't show if you're the player connecting
            Debug.Log(other.NickName + " connected");
            users.Add(other);
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log(other.NickName + " disconnected");
            users.Remove(other);
        }
    }
}