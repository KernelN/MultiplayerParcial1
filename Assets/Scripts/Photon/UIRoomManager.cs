using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class UIRoomManager : MonoBehaviourPunCallbacks
    {
        [Header("Set Values")]
        [SerializeField] PhotonRoomManager manager;
        [SerializeField] GameObject playerContainerPrefab;
        [SerializeField] RectTransform playersPanel;
        [SerializeField] RectTransform firstSpawn;
        [SerializeField] Color hostColor;
        [Header("Runtime Values")]
        [SerializeField] List<TextMeshProUGUI> containerList;
        [SerializeField] List<Player> playerList;
        [SerializeField] float distanceBetweenPlayers;

        //Unity Events
        private void Start()
        {
            if (!manager)
            {
                manager = PhotonRoomManager.Get();
            }

            //Initialize List
            containerList = new List<TextMeshProUGUI>();
            playerList = new List<Player>();

            //Set how much space is available as total space needed
            distanceBetweenPlayers = playersPanel.rect.height;
            //Divide by players * sizeOfContainer (how much space will all players occupy)
            distanceBetweenPlayers /= manager.publicMaxUsers * playerContainerPrefab.GetComponent<RectTransform>().rect.height; 
            //Add extra
            distanceBetweenPlayers *= 1.1f;

            //Create list (as we are already in a room)
            CreatePlayerList();
        }

        //Methods
        void CreatePlayerEntry(Player player)
        {
            //Instantiate Container
            GameObject container = Instantiate(playerContainerPrefab, playersPanel);

            //Set Position
            Vector2 pos = firstSpawn.rect.position;
            pos.y -= distanceBetweenPlayers * containerList.Count;
            RectTransform rectTransform = container.GetComponent<RectTransform>();
            rectTransform.rect.Set(pos.x, pos.y, rectTransform.rect.width, rectTransform.rect.);

            //Set player name
            TextMeshProUGUI text = container.GetComponentInChildren<TextMeshProUGUI>();
            text.text = player.NickName;

            //Set speecial color if player is host
            if (player.IsMasterClient)
            {
                text.color = hostColor;
            }

            //Add to list
            containerList.Add(text);
            playerList.Add(player);
        }
        void CreatePlayerList()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                CreatePlayerEntry(player.Value);
            }
        }
        void AddPlayerToList(Player player)
        {
            CreatePlayerEntry(player);
        }
        void RemovePlayerFromList(Player player)
        {
            //Get old player Container
            TextMeshProUGUI textToReplace = containerList[playerList.IndexOf(player)];

            //Get Last player in list Container
            TextMeshProUGUI textToRemove = containerList[containerList.Count - 1];
            Player playerToMove = playerList[containerList.IndexOf(textToRemove)];

            //Replace old player's text with the new one
            textToReplace.text = textToRemove.text;
            textToReplace.color = textToRemove.color;

            //Remove old values
            containerList.Remove(textToRemove);
            playerList.Remove(player);

            //Move player
            playerList.Remove(playerToMove); //remove from old position
            playerList.Insert(containerList.IndexOf(textToReplace), playerToMove); //add in new
        }

        //Photon Events (all this ARE technically, TCP)
        public override void OnPlayerEnteredRoom(Player other)
        {
            AddPlayerToList(other);
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            RemovePlayerFromList(other);
        }
    }
}