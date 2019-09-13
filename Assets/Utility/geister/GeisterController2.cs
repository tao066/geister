using System;
using System.Text.RegularExpressions;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HTTP;
using Protocol;

public class GeisterController2 : MonoBehaviour
{
    // -----------------------------------------------------------------
    // パネルオブジェクトを入れる変数 
    public GameObject BoardPanelObject;
    public GameObject MyselfPanelObject;
    public GameObject OpponentPanelObject;

    // パネルコントローラを入れる変数
    private BoardPanelController BoardPanelController;
    private CoinPanelController MyselfPanelController;
    private CoinPanelController OpponentPanelController;

    // 選択したコイン置き場のオブジェクトを入れる変数
    private GameObject StartCoinPlace;
    private GameObject EndCoinPlace;

    // 選択したコイン置き場のコントローラを入れる変数
    private CoinPlaceController StartCoinPlaceController;
    private CoinPlaceController EndCoinPlaceController;

    // Myself player のターンかどうか 
    private bool is_myself_turn = true;

    // 「始点かどうか」のデータを入れる変数
    private bool is_start_set = false;

    // マッチング中か
    private bool is_matching = true;

    private float delta = 0;

    private bool is_first = true;

    private string game_status = "exited";

    private bool game_started = false;

    private int turn_counta = 1;
    private int b_turn_counta = 1;
    // -----------------------------------------------------------------

    void Start()
    {
        // 各パネルから CoinPanelController を取得
        BoardPanelController = BoardPanelObject.GetComponent<BoardPanelController>();
        MyselfPanelController = MyselfPanelObject.GetComponent<CoinPanelController>();
        OpponentPanelController = OpponentPanelObject.GetComponent<CoinPanelController>();

        // 各コインパネル内にコイン置き場を用意する
        BoardPanelController.CreateCoinPlace(1);
        OpponentPanelController.CreateCoinPlace(2);
        MyselfPanelController.CreateCoinPlace(3);

        // 各プレイヤーのコインケースにコインを配置する
        MyselfPanelController.InitialiseSetCoin();
        OpponentPanelController.InitialiseSetCoin();
    }

    void Update()
    {
        bool bm = is_matching ? true : false;

        delta += Time.deltaTime;

        if (delta > 1.0f)
        {
            LoadRoomInfo();
            if (PlayerSession.game_id != 0)
                RoadGameInfo();
            if (b_turn_counta < turn_counta)
                RoadPieces();
            if (game_status == "playing" && !game_started)
            {
                game_started = true;
                RoadPieces();
            }
            delta = 0;
        }
    }

    // 移動の始点と終点のマスを取得して、プレイヤーやフェイズ、ターンに合わせて処理をする。
    public void SetCoinPlace(GameObject CoinPlaceObject)
    {
        // 以下の場合は操作不能とする
        // * マッチングできていない
        // * スタートしていない
        if (is_matching) return;
        if (game_status != "preparing" && !game_started) return;

        if (!is_start_set)
        {
            StartCoinPlace = CoinPlaceObject;
            StartCoinPlaceController = StartCoinPlace.GetComponent<CoinPlaceController>();

            // コインがないマスを選択できないようにする
            if (StartCoinPlaceController.coin_id == 0) return;

            // 相手の手持ちのパネルを選択できないようにする
            if (StartCoinPlaceController.panel_id == 2) return;

            is_start_set = true;

            BoardPanelController.HoverInitialiseSetPlace();
        }
        else
        {
            EndCoinPlace = CoinPlaceObject;
            EndCoinPlaceController = EndCoinPlace.GetComponent<CoinPlaceController>();

            is_start_set = false;

            BoardPanelController.DeleteHoverSetPlace();

            // フェイズ分岐
            switch (game_status)
            {
                case "preparing":
                    // 重複が発生した場合、動作を行わない。
                    if (CheckIsOverlap()) return;

                    // 手持ちからボードに移す場合の条件処理
                    if (CheckIsSendToBoard()) return;

                    break;

                case "playing":
                    // 自分自身のターンの場合の動作処理
                    if (CheckIsMyselfTurn()) return;

                    break;
            }
        }
    }
    // -----------------------------------------------------------------

    // 重複が発生した場合、true
    bool CheckIsOverlap()
    {
        return EndCoinPlaceController.coin_id != 0;
    }

    // 手持ちからボードに移す場合の条件処理
    bool CheckIsSendToBoard()
    {
        if (StartCoinPlaceController.panel_id == 3 && EndCoinPlaceController.panel_id == 1)
        {
            string width_str = "" + EndCoinPlaceController.width_id + "";
            string height_str = "" + EndCoinPlaceController.height_id + "";

            if (Regex.IsMatch(width_str, "[3456]") && Regex.IsMatch(height_str, "[12]"))
            {
                MoveToEmptyPlace();

                // セットが完了していれば、相手のターンに移行
                if (BoardPanelController.IsCompleteInitialiseSetPlace())
                {
                    // 初期配置を送信
                    SendPrepareGame();

                    is_myself_turn = false;
                }
                return true;
            }
        }

        return false;
    }

    // 自分自身のターンの場合の条件処理
    bool CheckIsMyselfTurn()
    {
        if (StartCoinPlaceController.panel_id == 1 && EndCoinPlaceController.panel_id == 1)
        {
            if (!StartCoinPlaceController.is_myplayer_piece) return false;
            if (EndCoinPlaceController.is_myplayer_piece) return false;

            int sw = StartCoinPlaceController.width_id, sh = StartCoinPlaceController.height_id;
            int ew = EndCoinPlaceController.width_id, eh = EndCoinPlaceController.height_id;

            if ((ew == sw && eh - 1 <= sh && sh <= eh + 1) || ((ew - 1 == sw || sw == ew + 1) && (eh == sh)))
            {
                MoveToPlayerPlace();
                return true;
            }
        }

        return false;
    }


    // idou
    void MoveToEmptyPlace()
    {
        EndCoinPlaceController.UpdateCoinPlace(StartCoinPlaceController.coin_id);
        StartCoinPlaceController.UpdateCoinPlace(0);
    }

    void MoveToPlayerPlace()
    {
        EndCoinPlaceController.UpdateCoinPlace(StartCoinPlaceController.coin_id);
        StartCoinPlaceController.UpdateCoinPlace(0);

        EndCoinPlaceController.is_myplayer_piece = true;
        StartCoinPlaceController.is_myplayer_piece = false;

        EndCoinPlaceController.piece_id = StartCoinPlaceController.piece_id;
        StartCoinPlaceController.piece_id = 0;

        SendMovePiece();
    }

    // room info syutoku
    void LoadRoomInfo()
    {
        RequestShowRoom param = new RequestShowRoom();

        param.room_id = PlayerSession.room_id;

        ApiClient.Instance.ResponseShowRoom = ResponseShowRoom;
        ApiClient.Instance.RequestShowRoom(param);
    }

    // roominfo res
    public void ResponseShowRoom(ResponseShowRoom response)
    {
        PlayerSession.game_id = response.game_id;
        is_matching = response.status == "waiting";
        return;
    }

    /// <summary>
    /// game info 
    /// </summary>
    void RoadGameInfo()
    {
        RequestShowGame param = new RequestShowGame();

        param.game_id = PlayerSession.game_id;

        ApiClient.Instance.ResponseShowGame = ResponseShowGame;
        ApiClient.Instance.RequestShowGame(param);
    }

    /// <summary>
    /// game info res
    /// </summary>
    /// <param name="response"></param>
    public void ResponseShowGame(ResponseShowGame response)
    {
        is_myself_turn = PlayerSession.user_id == response.turn_mover_user_id;
        is_first = PlayerSession.user_id == response.first_mover_user_id;
        game_status = response.status;
        turn_counta = response.turn_count;
    }

    /// <summary>
    /// syoki settei 
    /// send
    /// </summary>
    void SendPrepareGame()
    {
        RequestPrepareGame param = new RequestPrepareGame();

        param.game_id = PlayerSession.game_id;

        param.piece_preparations = new List<PiecePreparationInfo>();
        int i = 0;

        foreach (CoinPlaceController coinPlace in BoardPanelController.GetCoinPlaceControllers())
        {
            string width_str = "" + coinPlace.width_id + "";
            string height_str = "" + coinPlace.height_id + "";

            if (Regex.IsMatch(width_str, "[3456]") && Regex.IsMatch(height_str, "[12]"))
            {
                PiecePreparationInfo temp = new PiecePreparationInfo();

                temp.point_x = coinPlace.width_id - 1;
                temp.point_y = coinPlace.height_id;
                if (!is_first)
                {
                    switch (temp.point_x)
                    {
                        case 2: temp.point_x = 5; break;
                        case 3: temp.point_x = 4; break;
                        case 4: temp.point_x = 3; break;
                        case 5: temp.point_x = 2; break;
                    }
                    if (temp.point_y == 1)
                        temp.point_y = 6;
                    else
                        temp.point_y = 5;
                }
                temp.kind = coinPlace.Kind();

                // Debug.Log("x: " + temp.point_x + ", y: " + temp.point_y + ", kind: " + temp.kind);
                param.piece_preparations.Add(temp);
            }
        }
        Debug.Log(param.piece_preparations);
        ApiClient.Instance.ResponsePrepareGame = ResponsePrepareGame;
        ApiClient.Instance.RequestPrepareGame(param);
    }

    /// <summary>
    /// syoki settei send
    /// </summary>
    /// <param name="response"></param>
    public void ResponsePrepareGame(ResponsePrepareGame response)
    {
        Debug.Log("Prepare Complete");
        return;
    }

    void RoadPieces()
    {
        RequestListPieces param = new RequestListPieces();

        param.game_id = PlayerSession.game_id;

        ApiClient.Instance.ResponseListPieces = ResponseListPieces;
        ApiClient.Instance.RequestListPieces(param);
    }

    public void ResponseListPieces(ResponseListPieces response)
    {
        BoardPanelController.UpdateCoinPanel(response.pieces, is_first);
    }

    void SendMovePiece()
    {
        RequestUpdatePiece param = new RequestUpdatePiece();

        param.piece_id = EndCoinPlaceController.piece_id;
        param.point_x = EndCoinPlaceController.width_id - 1;
        param.point_y = EndCoinPlaceController.height_id;



        ApiClient.Instance.ResponseUpdatePiece = ResponseUpdatePiece;
        ApiClient.Instance.RequestUpdatePiece(param);
    }

    public void ResponseUpdatePiece(ResponseUpdatePiece response)
    {
        is_myself_turn = false;
    }
}
