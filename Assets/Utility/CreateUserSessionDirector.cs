using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using HTTP;
using Protocol;


public class CreateUserSessionDirector : MonoBehaviour {
    public InputField name_field;
    public InputField password_field;

    public void CreateUserSession()
    {
        ApiClient.Instance.SetIpAddress("http://127.0.0.1:3000");

        RequestCreateUserSession param = new RequestCreateUserSession();
        param.name     = name_field.text;
        param.password = password_field.text;

        ApiClient.Instance.ResponseCreateUserSession = ResponseCreateUserSession;
        ApiClient.Instance.RequestCreateUserSession(param);
    }

    // Callback
    public void ResponseCreateUserSession(ResponseCreateUserSession response)
    {
        PlayerSession.user_session_id = response.user_session_id;
        PlayerSession.access_token    = response.access_token;
        PlayerSession.user_id         = response.user_id;

        Debug.Log(PlayerSession.user_session_id);
        Debug.Log(PlayerSession.access_token);
        Debug.Log(PlayerSession.user_id);

        SceneManager.LoadScene("room_scene");
    }
}
