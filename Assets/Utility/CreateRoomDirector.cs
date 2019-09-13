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
        RequestCreateRoom param = new RequestCreateRoom();

        ApiClient.Instance.ResponseCreateRoom = ResponseCreateRoom;
        ApiClient.Instance.RequestCreateRoom(param);
    }

    public void ResponseCreateRoom(ResponseCreateRoom response)
    {
        PlayerSession.room_id = response.room_id;
        PlayerSession.player_entry_id = response.player_entry_id;
        PlayerSession.is_owner = true;

        Debug.Log("create: room_id[" + response.room_id + "]");
        Debug.Log("Player \"" + PlayerSession.name + "\" Create and Entry to room number \"" + response.room_id + "\"");

        SceneManager.LoadScene("battle_scene");
    }
}
