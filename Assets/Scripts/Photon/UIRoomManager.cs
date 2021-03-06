using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Photon
{
    public class UIRoomManager : MonoBehaviourPunCallbacks
    {
        [Header("Set Values")]
        [SerializeField] PhotonRoomManager manager;
        [SerializeField] GameObject playerContainerPrefab;
        [SerializeField] RectTransform playersPanel;
        [SerializeField] RectTransform firstSpawn;
        [SerializeField] Button playButton;
        [SerializeField] Image playImage;
        [SerializeField] Color hostColor;
        [Header("Runtime Values")]
        [SerializeField] List<TextMeshProUGUI> containerList;
        [SerializeField] List<Player> playerList;
        [SerializeField] float distanceBetweenPlayers;

        //Unity Events
        private void Start()
        {
            manager = PhotonRoomManager.Get();

            //If we're host, call on Room Joined
            if (PhotonNetwork.IsMasterClient)
            {
                OnRoomJoined();
            }
            else
            {
                manager.RoomJoined += OnRoomJoined;
            }
            Debug.Log("Linked");
        }

        //Methods
        void SetButton()
        {
            if (PhotonNetwork.IsMasterClient) return;
            playButton.enabled = false;
            playImage.color = Color.gray;
        }
        void GetContainerDistance()
        {
            //Set how much space is available as total space needed
            distanceBetweenPlayers = playersPanel.rect.height;
            //Debug.Log("Panel Y: " + playersPanel.rect.height);

            //Divide by max players * sizeOfContainer (how much space will all players occupy)
            float containerSize = playerContainerPrefab.GetComponent<RectTransform>().rect.height;
            distanceBetweenPlayers /= manager.publicMaxUsers * containerSize;
            //Debug.Log("Container Y: " + containerSize);

            //Add extra
            distanceBetweenPlayers *= 1.1f;

            //Set Spawner Pos
            Vector2 pos = firstSpawn.anchoredPosition;
            pos.y -= distanceBetweenPlayers + containerSize / 2;
            firstSpawn.anchoredPosition = pos;

            //Add container size to min distance
            if (distanceBetweenPlayers < containerSize)
            {
                distanceBetweenPlayers += containerSize;
            }
        }
        void CreatePlayerEntry(Player player)
        {
            //Instantiate Container
            GameObject container = Instantiate(playerContainerPrefab, playersPanel);

            //Set Position
            RectTransform rect = container.GetComponent<RectTransform>();
            Vector2 pos = firstSpawn.anchoredPosition;
            pos.y -= distanceBetweenPlayers * containerList.Count;
            rect.anchoredPosition = pos;
            //rectTransform.rect.Set(pos.x, pos.y, rectTransform.rect.width, rectTransform.rect.height);

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
            //Get Player to Remove Index
            int playerIndex = playerList.IndexOf(player);

            //Get old player Container
            TextMeshProUGUI textToReplace = containerList[playerIndex];

            //If Player is in last position, just remove
            if (playerIndex == playerList.Count - 1)
            {
                containerList.Remove(textToReplace);
                playerList.Remove(player);
                Destroy(textToReplace.transform.parent.gameObject);
                return;
            }
            
            //Get Last player in list Container
            TextMeshProUGUI textToRemove = containerList[containerList.Count - 1];
            Player playerToMove = playerList[containerList.IndexOf(textToRemove)];

            //Replace old player's text with the new one
            textToReplace.text = textToRemove.text;
            textToReplace.color = textToRemove.color;

            //Remove old values
            containerList.Remove(textToRemove);
            Destroy(textToRemove.transform.parent.gameObject); //remove Container, not only text
            playerList.Remove(player);

            //Move player
            playerList.Remove(playerToMove); //remove from old position
            int textToReplaceIndex = containerList.IndexOf(textToReplace);
            playerList.Insert(textToReplaceIndex, playerToMove); //add in new
        }
        void UpdateHostNick()
        {
            foreach (var player in playerList)
            {
                if (!player.IsMasterClient) continue;
                int hostIndex = playerList.IndexOf(player);
                containerList[hostIndex].color = hostColor;
            }
        }
        public void StartGame()
        {
            manager.LoadLevel();
        }

        //Photon Events
        public override void OnPlayerEnteredRoom(Player other)
        {
            AddPlayerToList(other);
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            bool quitterWasHost = other.IsMasterClient;

            //Remove Player from the list (UI)
            RemovePlayerFromList(other);

            ////If player who left was the host, update color of new host
            //if (!quitterWasHost) return;
            UpdateHostNick();
        }

        //Event Receivers
        void OnRoomJoined()
        {
            Debug.Log("Initialized");
            
            //Initialize List
            containerList = new List<TextMeshProUGUI>();
            playerList = new List<Player>();

            //Set distance between containers
            GetContainerDistance();

            //Modify Button so only Host can start Playing
            SetButton();

            //Create list (as we are already in a room)
            CreatePlayerList();
        }
    }
}