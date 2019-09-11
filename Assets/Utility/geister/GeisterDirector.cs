using System;
using System.Text.RegularExpressions;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HTTP;
using Protocol;

public class GeisterDirector : MonoBehaviour {

    public GameObject board_panel;
    public GameObject myself_coin_panel;
    public GameObject opponent_coin_panel;

    private BoardPanelController BoardPanelController;
    private CoinPanelController  MyselfPanelController;
    private CoinPanelController  OpponentPanelController;

    private GameObject StartCoinPlace;
    private GameObject EndCoinPlace;
    private CoinPlaceController start_coin_place;
    private CoinPlaceController end_coin_place;
    private bool is_start_set = false;

    // 移動の始点と終点のマスを取得して、プレイヤーやフェイズ、ターンに合わせて処理をする。
    public void SetStartCoinPlace(GameObject coin_place)
    {
        if (!is_start_set)
        {
            StartCoinPlace = coin_place;
            start_coin_place = StartCoinPlace.GetComponent<CoinPlaceController>();

            // コインがないマスを選択できないようにする
            if (start_coin_place.coin_id  == 0) return;

            // 相手のパネルを選択できないようにする
            if (start_coin_place.panel_id == 2) return;

            is_start_set = true;
            Debug.Log("選択中");
        }
        else
        {
            EndCoinPlace = coin_place;
            Debug.Log("start panel_id:" + StartCoinPlace.name + ", End panel_id: " + EndCoinPlace.name);

            start_coin_place = StartCoinPlace.GetComponent<CoinPlaceController>();
            end_coin_place   = EndCoinPlace.GetComponent<CoinPlaceController>();

            is_start_set = false;

            // 重複が発生した場合、動作を行わない。
            if ( CheckIsOverlap() ) return;

            // 手持ちからボードに移す場合の条件処理
            if ( CheckIsSendToBoard() ) return;

            // 自分自身のターンの場合の条件処理
            if ( CheckIsMyselfTurn() ) return;
        }
    }

    // 重複が発生した場合、動作を行わない。
    bool CheckIsOverlap()
    {
        return end_coin_place.coin_id != 0;
    }

    // 手持ちからボードに移す場合の条件処理
    bool CheckIsSendToBoard()
    {
        if (start_coin_place.panel_id == 3 && end_coin_place.panel_id == 1)
        {
            string width_str  = "" + end_coin_place.width_id  + "";
            string height_str = "" + end_coin_place.height_id + "";

            if (Regex.IsMatch(width_str ,"[3456]") && Regex.IsMatch(height_str, "[12]"))
            {
                MoveToEmptyPlace();
                return true;
            }
        }

        return false;
    }

    // 自分自身のターンの場合の条件処理
    bool CheckIsMyselfTurn()
    {
        if (start_coin_place.panel_id == 1 && end_coin_place.panel_id == 1)
        {
            int sw = start_coin_place.width_id, sh = start_coin_place.height_id;
            int ew = end_coin_place.width_id, eh = end_coin_place.height_id;

            if ( (ew == sw && eh -1 <= sh && sh <= eh + 1) || ((ew - 1 == sw || sw == ew + 1) && (eh == sh)))
            {
                MoveToEmptyPlace();
                return true;
            }
        }

        return false;
    }

    void MoveToEmptyPlace()
    {
        end_coin_place.UpdateCoinPlace(start_coin_place.coin_id);
        start_coin_place.UpdateCoinPlace(0);
    }

    void Start () {
        // 各パネルから CoinPanelController を取得
        BoardPanelController = board_panel.GetComponent<BoardPanelController>();
        MyselfPanelController   = myself_coin_panel.GetComponent<CoinPanelController>();
        OpponentPanelController = opponent_coin_panel.GetComponent<CoinPanelController>();

        // 各コインパネル内にコイン置き場を用意する
        BoardPanelController.CreateCoinPlace(1);
        MyselfPanelController.CreateCoinPlace(2);
        OpponentPanelController.CreateCoinPlace(3);

        // 各プレイヤーのコインケースにコインを配置する
        // BoardPanelController.InitialiseSetCoin();
        MyselfPanelController.InitialiseSetCoin();
        OpponentPanelController.InitialiseSetCoin();
    }
}
