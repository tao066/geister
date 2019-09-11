using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using HTTP;
using Protocol;


public class CreateRoomDirector : MonoBehaviour
{

    public void CreateRoom()
    {
        ApiClient.Instance.SetIpAddress("http://127.0.0.1:3000");
        ApiClient.Instance.SetAccessToken(PlayerSession.access_token);

        RequestCreateRoom param = new RequestCreateRoom();

        ApiClient.Instance.ResponseCreateRoom = ResponseCreateRoom;
        ApiClient.Instance.RequestCreateRoom(param);
    }

    public void ResponseCreateRoom(ResponseCreateRoom response)
    {
        PlayerSession.player_entry_id = response.player_entry_id;

        Debug.Log("create: room_id[" + response.room_id + "]");
        Debug.Log("Player \"" + PlayerSession.name + "\" Create and Entry to room number \"" + response.room_id + "\"");

        SceneManager.LoadScene("battle_scene");
    }
}
