using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MultiplayerGame.Photon
{
    public class PhotonNicknameInputter : MonoBehaviour
    {
        [SerializeField] TMP_InputField inputField;

        //Methods
        public void SetPlayerNick()
        {
            //Get input's text
            string value = inputField.text;

            //If string is empty, skip
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError(gameObject + "'s Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;
            Debug.Log("Inputted " + value);
        }
    }
}