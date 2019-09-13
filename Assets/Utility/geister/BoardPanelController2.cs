using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol;

public class BoardPanelController2 : MonoBehaviour
{

    public GameObject coin_place_prefab;

    public void CreateCoinPlace(int panel_id)
    {
        int width = 8;
        int height = 6;

        for (int j = 1; j <= height; j++)
        {
            for (int i = 1; i <= width; i++)
            {
                if ((j > 1 && j < height) && (i == 1 || i == width)) continue;
                GameObject go = Instantiate(coin_place_prefab, transform);

                go.name = "coin_place_clone_" + j + "" + i;
                go.transform.Translate(i * 80 - 80, j * 80 - 80, 0);
                go.GetComponent<CoinPlaceController>().SetPlaceId(1, i, j);
            }
        }
    }

    public void UpdateCoinPanel(List<PieceInfo> pieces, bool is_first)
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                GameObject go = child.gameObject;
                CoinPlaceController coin = go.GetComponent<CoinPlaceController>();

                coin.piece_id = 0;
                coin.is_myplayer_piece = false;
                coin.DeleteCoin();
            }
        }

        foreach (PieceInfo piece in pieces)
        {
            if (piece.captured) continue;

            int width = piece.point_x + 1;
            int height = piece.point_y;

            if (!is_first)
            {
                width = 9 - width;
                height = 7 - height;
            }

            Debug.Log("coin_place_clone_" + height + "" + width);
            GameObject go = transform.Find("coin_place_clone_" + height + "" + width).gameObject;

            CoinPlaceController init_coin = go.GetComponent<CoinPlaceController>();

            init_coin.piece_id = piece.piece_id;
            init_coin.is_myplayer_piece = piece.kind != "unknown";
            init_coin.UpdateCoinPlace(init_coin.CoinId(piece.kind));
        }
    }

    /// <summary>
    /// 全てのパネルを返す
    /// </summary>
    /// <returns></returns>
    public List<CoinPlaceController> GetCoinPlaceControllers()
    {
        List<CoinPlaceController> res = new List<CoinPlaceController>();

        foreach (Transform CoinPlace in transform)
        {
            CoinPlaceController CoinPlaceController = CoinPlace.gameObject.GetComponent<CoinPlaceController>();
            res.Add(CoinPlaceController);
        }

        return res;
    }

    /// <summary>
    /// 全てのパネルの着色を消す
    /// </summary>
    public void DeleteHoverSetPlace()
    {
        foreach (Transform CoinPlace in transform)
        {
            CoinPlaceController CoinPlaceController = CoinPlace.gameObject.GetComponent<CoinPlaceController>();
            CoinPlace.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
    }

    /// <summary>
    /// 初期配置できる範囲内のパネルを着色する
    /// </summary>
    public void HoverInitialiseSetPlace()
    {
        foreach (Transform CoinPlace in transform)
        {
            CoinPlaceController CoinPlaceController = CoinPlace.gameObject.GetComponent<CoinPlaceController>();

            string width_str = "" + CoinPlaceController.width_id + "";
            string height_str = "" + CoinPlaceController.height_id + "";

            if (Regex.IsMatch(width_str, "[3456]") && Regex.IsMatch(height_str, "[12]"))
            {
                if (CoinPlaceController.coin_id == 0)
                    CoinPlace.gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f, 100f / 255f);
            }
        }
    }

    /// <summary>
    /// 初期配置がすべて完了してるかどうかを返す
    /// </summary>
    /// <returns></returns>
    public bool IsCompleteInitialiseSetPlace()
    {
        foreach (Transform CoinPlace in transform)
        {
            CoinPlaceController CoinPlaceController = CoinPlace.gameObject.GetComponent<CoinPlaceController>();

            string width_str = "" + CoinPlaceController.width_id + "";
            string height_str = "" + CoinPlaceController.height_id + "";

            if (Regex.IsMatch(width_str, "[3456]") && Regex.IsMatch(height_str, "[12]"))
            {
                if (CoinPlaceController.coin_id == 0)
                    return false;
            }
        }

        return true;
    }
}
