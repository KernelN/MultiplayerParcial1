using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Universal.SceneManaging;

namespace MultiplayerGame.Photon
{
    public class PhotonRoomManager : Singletons.PunSingleton<PhotonRoomManager>
    {
        [Header("Set Values")]
        [SerializeField] [Range(1, 20)] byte maxPlayersPerRoom = 4;
        [Header("Runtime Values")]
        public PhotonRoomChecker roomChecker;
        public Scenes currentScene;
        public bool disconnected;
        [SerializeField] List<Player> users;
        [SerializeField] int userNumber;

        public System.Action RoomJoined;
        public System.Action<Scenes> SceneChanged;

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
            Debug.Log("Loading Lobby Room");
            PhotonNetwork.LoadLevel("Room");
            currentScene = Scenes.roomLobby;
            SceneChanged?.Invoke(Scenes.roomLobby);
        }
        public void LoadLevel()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Debug.Log("Loading Game");
            PhotonNetwork.LoadLevel("GameplayHost");
            currentScene = Scenes.roomLobby;
            SceneChanged?.Invoke(Scenes.gameplay);
        }


        //Photon Events
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(gameObject + " No random room available, create one");

            //No random room available, create one
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        public override void OnJoinedRoom()
        {
            //Set disconnected to false and update current room
            disconnected = false;

            //Get user number and Set nick if needed
            userNumber = PhotonNetwork.CurrentRoom.PlayerCount;
            if (PhotonNetwork.NickName == "")
            {
                string nick = "#" + PhotonNetwork.LocalPlayer.ActorNumber;
                PhotonNetwork.NickName = nick;
                PhotonNetwork.LocalPlayer.NickName = nick;
            }

            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                users.Add(player.Value);
            }
            LoadGameRoom();
            RoomJoined?.Invoke();
        }
        public override void OnLeftRoom()
        {
            users.Clear();
            disconnected = true;
            GameManager.Get().LoadScene(Universal.SceneManaging.Scenes.lobby);
        }
        public override void OnPlayerEnteredRoom(Player other)
        {
            //Doesn't show if you're the player connecting
            Debug.Log(other.NickName + " connected");
            users.Add(other);
            if (other.NickName == "")
            {
                string nick = "#" + other.ActorNumber;
                other.NickName = nick;                
            }
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log(other.NickName + " disconnected");
            users.Remove(other);
            if (currentScene == Scenes.gameplay)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
    }
}