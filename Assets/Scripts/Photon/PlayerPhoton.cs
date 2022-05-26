using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class PlayerPhoton : MonoBehaviourPunCallbacks
    {
        [SerializeField] bool isPlayable;

        //Unity Events
        private void Start()
        {
            //If is host but is not using the playable prefab
            if (photonView.IsMine != isPlayable)
            {
                //do magic
            }
        }

        //Methods
    }
}