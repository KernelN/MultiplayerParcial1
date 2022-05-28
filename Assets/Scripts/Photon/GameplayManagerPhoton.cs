using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class GameplayManagerPhoton : MonoBehaviour
    {
        [Header("Set Values")]
        //[SerializeField] Gameplay.GameplayManager manager;
        [SerializeField] PhotonRoomManager roomManager;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] Transform spawnCenter;
        [SerializeField] Vector2 spawnRadius;
        [Header("Runtime Values")]
        [SerializeField] int totalPlayers;
        [SerializeField] int playerNumber;

        //Unity Events
        private void Start()
        {
            //Get Room Manager
            if (!roomManager)
            {
                roomManager = PhotonRoomManager.Get();
            }
            
            //Get player numbers (of room)
            totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
            playerNumber = roomManager.publicUserNumber;

            //Create player
            GameObject player = InstantiatePlayer();
            SetColor(player.GetComponent<SpriteRenderer>());
            player.name = PhotonNetwork.NickName;
        }

        //Methods
        GameObject InstantiatePlayer()
        {
            string prefab = playerPrefab.name;
            return PhotonNetwork.Instantiate(prefab, GetPlayerPos(), transform.rotation);
        }
        Vector2 GetPlayerPos()
        {
            //Get how many players will be per row (row x column = players)
            float playerPerRow = Mathf.Sqrt(totalPlayers);

            //Calculate distances
            float xDistance = (spawnRadius.x) / playerPerRow;
            float yDistance = (spawnRadius.y) / playerPerRow;

            //Set First position
            float xPos = transform.position.x - spawnRadius.x;
            float yPos = transform.position.y - spawnRadius.y;

            //Move equally to player number
            xPos += xDistance * roomManager.publicUserNumber;

            //Fix position until is in area
            while (xPos > spawnRadius.x)
            {
                xPos -= spawnRadius.x;
                yPos += yDistance;
            }

            return new Vector2(xPos, yPos);
        }
        void SetColor(SpriteRenderer renderer)
        {
            Color newColor = renderer.material.color;
            float colorDistance = 1 / ((float)totalPlayers / 3);
            float colorPool = colorDistance * playerNumber;
            int currentColor = 1;
            do
            {
                //Add Color to player
                switch (currentColor)
                {
                    case 1:
                        newColor.r += colorDistance;
                    break;
                    case 2:
                        newColor.g += colorDistance;
                        break;
                    case 3:
                        newColor.b += colorDistance;
                        break;
                    default:
                        break;
                }

                //Decrease color pool
                colorPool -= colorDistance;
                
                //Increase color index
                currentColor++;
                if (currentColor > 3) currentColor = 0;
            } while (colorPool > 0);
            renderer.material.color = newColor;
        }
    }
}
