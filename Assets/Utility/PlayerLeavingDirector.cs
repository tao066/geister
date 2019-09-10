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

        RequestDeletePlayer param = new RequestDeletePlayer();

        param.user_session_id = PlayerSession.user_session_id;

        ApiClient.Instance.ResponseDeleteUserSession = ResponseDeleteUserSession;
        ApiClient.Instance.RequestDeleteUserSession(param);
    }

    public void ResponseDeleteUserSession(ResponseDeleteUserSession response)
    {
        PlayerSession.user_session_id = 0;
        PlayerSession.access_token = "";
        PlayerSession.user_id = 0;

        Debug.Log("Logout: " + response.name);

        SceneManager.LoadScene("title_scene");
    }
}
