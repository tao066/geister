using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using HTTP;
using Protocol;

public class DeleteUserSessionDirector : MonoBehaviour {

	public void DeleteUserSession() {
        RequestDeleteUserSession param = new RequestDeleteUserSession();

        param.user_session_id = PlayerSession.user_session_id;

        ApiClient.Instance.ResponseDeleteUserSession = ResponseDeleteUserSession;
        ApiClient.Instance.RequestDeleteUserSession(param);
    }
	
	public void ResponseDeleteUserSession(ResponseDeleteUserSession response) {
        Debug.Log("Logout: " + response.name);
        SceneManager.LoadScene("title_scene");
    }
}
