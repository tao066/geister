using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using HTTP;
using Protocol;

public class DeleteUserSessionDirector : MonoBehaviour {

	public void DeleteUserSession() {
        ApiClient.Instance.SetIpAddress("http://127.0.0.1:3000");

        RequestDeleteUserSession param = new RequestDeleteUserSession();

        param.user_session_id = PlayerSession.user_session_id;

        ApiClient.Instance.SetAccessToken(PlayerSession.access_token);
        ApiClient.Instance.ResponseDeleteUserSession = ResponseDeleteUserSession;
        ApiClient.Instance.RequestDeleteUserSession(param);
    }
	
	public void ResponseDeleteUserSession(ResponseDeleteUserSession response) {
        PlayerSession.user_session_id = 0;
        PlayerSession.access_token = "";
        PlayerSession.user_id = 0;

        Debug.Log("Logout: " + response.name);

        SceneManager.LoadScene("title_scene");
    }
}
