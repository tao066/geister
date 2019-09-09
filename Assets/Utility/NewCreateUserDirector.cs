using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HTTP;
using Protocol;


public class NewCreateUserDirector : MonoBehaviour {
    public InputField name_field;
    public InputField password_field;

    public void CreateNewUser()
    {
        // 通信先アドレスの設定
        ApiClient.Instance.SetIpAddress("http://127.0.0.1:3000");
        // RequestCreateUser 型の変数を宣言
        RequestCreateUser param = new RequestCreateUser();
        // ユーザ名（英数字4文字以上16文字以下）
        param.name = name_field.text;
        //パスワード（英数字8文字以上16文字以下）
        param.password = password_field.text;

        // Callback先の関数を設定する
        ApiClient.Instance.ResponseCreateUser = ResponseCreateUser;
        // リクエストを送る
        ApiClient.Instance.RequestCreateUser(param);
    }

    // Callback
    public void ResponseCreateUser(ResponseCreateUser response)
    {
        Debug.Log(response.user_id);
        Debug.Log(response.name);
    }

}
