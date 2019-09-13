using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Protocol;

public class CoinPanelController : MonoBehaviour
{
    public GameObject coin_place_prefab;

    public void CreateCoinPlace(int panel_id)
    {
        int width = 4;
        int height = 2;

        for (int j = 1; j <= height; j++)
        {
            for (int i = 1; i <= width; i++)
            {
                GameObject go = Instantiate(coin_place_prefab, transform);
                
                go.name = "coin_place_clone_"+ j +""+ i;
                go.transform.Translate(i * 80 - 80, j * 80 - 80, 0);
                go.GetComponent<CoinPlaceController>().SetPlaceId(panel_id, i, j);
            }
        }
    }

    public void InitialiseSetCoin()
    {
        foreach (Transform coin_place in transform)
        {
            CoinPlaceController coin_place_controller = coin_place.gameObject.GetComponent<CoinPlaceController>();

            if (coin_place_controller.panel_id == 2)
                coin_place_controller.SetCoinUra();
            else if (coin_place_controller.height_id == 1)
                coin_place_controller.SetCoinGood();
            else
                coin_place_controller.SetCoinEvil();
        }
    }

    public void UpdateCoinPanel(List<PieceInfo> pieces, bool is_player)
    {
        int user_id = PlayerSession.user_id;

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

        int counta = 0;

        foreach (PieceInfo piece in pieces)
        {
            if (!piece.captured) continue;
            if (piece.owner_user_id == user_id && !is_player) continue;
            if (piece.owner_user_id != user_id && is_player)  continue;

            counta += 1;

            int width = counta % 4;
            int height = counta / 4 + 1 ;

            GameObject go = transform.Find("coin_place_clone_" + height + "" + width).gameObject;

            CoinPlaceController init_coin = go.GetComponent<CoinPlaceController>();

            init_coin.piece_id = piece.piece_id;
            init_coin.is_myplayer_piece = false;
            init_coin.UpdateCoinPlace(init_coin.CoinId(piece.kind));
        }
    }
}