using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using HTTP;
using Protocol;

public class RoomListController : MonoBehaviour {

    public GameObject owner_name_object;
    public GameObject status_object;

    public int room_id;

    public void InputRoomInfo(RoomInfo room)
    {
        this.owner_name_object.GetComponent<Text>().text = room.owner_name;

        switch (room.status)
        {
            case "waiting": this.status_object.GetComponent<Text>().text = "待機中";    break;
            case "playing": this.status_object.GetComponent<Text>().text = "対戦中";    break;
            default       : this.status_object.GetComponent<Text>().text = room.status; break;
        }

        this.room_id = room.room_id;
    }

    public void PlayerEntry()
    {
        RequestCreatePlayerEntry param = new RequestCreatePlayerEntry();

        param.room_id = this.room_id;

        ApiClient.Instance.ResponseCreatePlayerEntry = ResponseCreatePlayerEntry;
        ApiClient.Instance.RequestCreatePlayerEntry(param);
    }

    public void ResponseCreatePlayerEntry(ResponseCreatePlayerEntry response)
    {
        PlayerSession.room_id = response.room_id;
        PlayerSession.player_entry_id = response.player_entry_id;
        PlayerSession.is_owner = false;

        Debug.Log("Player \"" + PlayerSession.name + "\" Entry to room number \"" + response.room_id + "\"");
        SceneManager.LoadScene("battle_scene");
    }
}
