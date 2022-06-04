using Photon.Pun;
using UnityEngine;
using Universal.SceneManaging;

namespace MultiplayerGame.Photon
{
    public class PhotonRoomChecker : MonoBehaviourPunCallbacks
    {
        [SerializeField] PhotonRoomManager manager;
        [SerializeField] Scenes currentScene;


        //Unity Events
        private void Start()
        {
            manager = PhotonRoomManager.Get();
            manager.roomChecker = this;
            manager.SceneChanged += OnSceneChanged;
            OnSceneChanged(manager.currentScene);
        }
        private void OnDestroy()
        {
            manager.roomChecker = null;
            manager.SceneChanged -= OnSceneChanged;
        }

        //EventReceivers
        [PunRPC]
        public void OnSceneChanged(Scenes newScene)
        {
            currentScene = newScene;
            manager.currentScene = currentScene;
            if (!PhotonNetwork.IsMasterClient) return;
            photonView.RPC("OnSceneChanged", RpcTarget.Others, newScene);
        }
    }
}