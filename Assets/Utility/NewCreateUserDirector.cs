using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HTTP;
using Protocol;


public class NewCreateUserDirector : MonoBehaviour {
    public InputField name_field;
    public InputField password_field;
    public InputField ip;

    public void CreateNewUser()
    {
        ApiClient.Instance.SetIpAddress(this.ip.text);
        PlayerSession.ip = this.ip.text;

        RequestCreateUser param = new RequestCreateUser();

        param.name = name_field.text;
        param.password = password_field.text;
        
        ApiClient.Instance.ResponseCreateUser = ResponseCreateUser;
        ApiClient.Instance.RequestCreateUser(param);
    }

    public void ResponseCreateUser(ResponseCreateUser response)
    {
        Debug.Log("Create User: " + response.name);
    }
}
