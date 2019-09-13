using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HTTP;
using Protocol;

public class RoomListGenerator : MonoBehaviour {

    public GameObject room_list_prefab;
    public GameObject room_list_area;

    public void InstantiateRoomList()
    {
        RequestListRooms param = new RequestListRooms();

        ApiClient.Instance.ResponseListRooms = ResponseListRooms;
        ApiClient.Instance.RequestListRooms(param);
    }

    public void ResponseListRooms(ResponseListRooms response)
    {
        foreach (RoomInfo room in response.rooms)
        {
            Debug.Log("room_id: " + room.room_id + ", status: " + room.status + ", game_id: " + room.game_id + ", owner_name: " + room.owner_name);
            GameObject go = Instantiate(room_list_prefab, room_list_area.transform);

            go.name = "room_list_clone_" + room.room_id;
            go.GetComponent<RoomListController>().InputRoomInfo(room);
        }
    }

    void Start()
    {
        InstantiateRoomList();
    }
}
