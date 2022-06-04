using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class PhotonSceneCheckerInstantiator : MonoBehaviour
    {
        [SerializeField] GameObject roomCheckerPrefab;

        //Unity Events
        private void Awake()
        {
            PhotonNetwork.Instantiate(roomCheckerPrefab.name, Vector3.zero, transform.rotation);
        }

        //Methods
    }
}