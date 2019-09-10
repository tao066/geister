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
        ApiClient.Instance.SetIpAddress("http://127.0.0.1:3000");
        ApiClient.Instance.SetAccessToken(PlayerSession.access_token);

        RequestCreatePlayerEntry param = new RequestCreatePlayerEntry();

        param.room_id = this.room_id;

        ApiClient.Instance.ResponseCreatePlayerEntry = ResponseCreatePlayerEntry;
        ApiClient.Instance.RequestCreatePlayerEntry(param);
    }

    public void ResponseCreatePlayerEntry(ResponseCreatePlayerEntry response)
    {
        Debug.Log("Player \"" + PlayerSession.name + "\" Entry to number \"" + response.room_id + "\"");
        SceneManager.LoadScene("battle_scene");
    }
}
