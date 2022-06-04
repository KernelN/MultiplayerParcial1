using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class AreaControllerPhoton : MonoBehaviourPunCallbacks
    {
        [SerializeField] Gameplay.AreaController controller;
        [SerializeField] PhotonRoomManager roomManager;

        //Unity Events
        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(controller);
                Destroy(this);
                return;
            }

            if (!controller)
            {
                controller = GetComponent<Gameplay.AreaController>();
            }
            if (!roomManager)
            {
                roomManager = PhotonRoomManager.Get();
            }

            controller.maxEntities = PhotonNetwork.CurrentRoom.PlayerCount;
        }

        //Photon Events
        public override void OnPlayerEnteredRoom(Player other)
        {
            controller.maxEntities = PhotonNetwork.CurrentRoom.PlayerCount;
            controller.CheckPool();
        }
    }
}