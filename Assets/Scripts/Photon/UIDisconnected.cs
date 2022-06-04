using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class UIDisconnected : MonoBehaviour
    {
        [SerializeField] PhotonRoomManager roomManager;
        [SerializeField] GameObject disconnectedAlert;

        //Unity Events
        private void Start()
        {
            roomManager = PhotonRoomManager.Get();
            disconnectedAlert.SetActive(roomManager.disconnected);
        }

        //Methods
    }
}