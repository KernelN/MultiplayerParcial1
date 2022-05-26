using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class PhotonRoomManager : Singletons.PunSingletonInScene<PhotonRoomManager>
    {
        [Header("Set Values")]
        [SerializeField] [Range(1, 20)] byte maxPlayersPerRoom = 4;

        //Unity Events

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
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Try to Load a level but not Host");
            }
            Debug.LogFormat(gameObject + "  Loading Game Room");
            PhotonNetwork.LoadLevel("Gameplay");
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
            Debug.Log("You joined room");
            LoadGameRoom();
        }
        public override void OnLeftRoom()
        {
            GameManager.Get().LoadScene(Universal.SceneManaging.Scenes.lobby);
        }
        public override void OnPlayerEnteredRoom(Player other)
        {
            //Doesn't show if you're the player connecting
            Debug.Log(other.NickName + " connected"); 
            //if (PhotonNetwork.IsMasterClient)
            //{
            //  
            //}
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log(other.NickName + " disconnected");
        }
    }
}