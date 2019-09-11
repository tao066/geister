using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using HTTP;
using Protocol;

public class PlayerLeavingDirector : MonoBehaviour {

    public void PlayerLeaving()
    {
        ApiClient.Instance.SetIpAddress("http://127.0.0.1:3000");
        ApiClient.Instance.SetAccessToken(PlayerSession.access_token);

        RequestDeletePlayerEntry param = new RequestDeletePlayerEntry();

        param.player_entry_id = PlayerSession.player_entry_id;

        ApiClient.Instance.ResponseDeletePlayerEntry = ResponseDeletePlayerEntry;
        ApiClient.Instance.RequestDeletePlayerEntry(param);
    }

    public void ResponseDeletePlayerEntry(ResponseDeletePlayerEntry response)
    {
        Debug.Log("Player \"" + PlayerSession.name + "\" Leaving to room number \"" + response.room_id + "\"");
        SceneManager.LoadScene("room_scene");
    }
}
