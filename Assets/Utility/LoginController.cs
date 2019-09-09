using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HTTP;
using Protocol;

public class LoginController : MonoBehaviour {

    void Start()
    {
        CreateNewUser();
    }

    public void CreateNewUser()
    {
        // 通信先アドレスの設定
        ApiClient.Instance.SetIpAddress("http://127.0.0.1:3000");
        // RequestCreateUser 型の変数を宣言
        RequestCreateUser param = new RequestCreateUser();
        // ユーザ名（英数字4文字以上16文字以下）
        param.name = "honoka";
        //パスワード（英数字8文字以上16文字以下）
        param.password = "fightdayo";

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
