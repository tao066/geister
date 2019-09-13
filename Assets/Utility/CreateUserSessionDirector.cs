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
    public InputField ip;

    public void CreateUserSession()
    {
        ApiClient.Instance.SetIpAddress(this.ip.text);
        PlayerSession.ip = this.ip.text;

        RequestCreateUserSession param = new RequestCreateUserSession();
        param.name     = name_field.text;
        param.password = password_field.text;

        PlayerSession.name = param.name;

        ApiClient.Instance.ResponseCreateUserSession = ResponseCreateUserSession;
        ApiClient.Instance.RequestCreateUserSession(param);
    }

    // Callback
    public void ResponseCreateUserSession(ResponseCreateUserSession response)
    {
        PlayerSession.user_session_id = response.user_session_id;
        PlayerSession.access_token    = response.access_token;
        PlayerSession.user_id         = response.user_id;

        ApiClient.Instance.SetAccessToken(PlayerSession.access_token);

        Debug.Log("Login: " + PlayerSession.name);

        SceneManager.LoadScene("room_scene");
    }
}
