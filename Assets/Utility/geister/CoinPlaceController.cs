using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPlaceController : MonoBehaviour
{

    public int panel_id, width_id, height_id, coin_id;

    public bool is_myplayer_piece;
    public int piece_id = 0;

    public GameObject coin_evil_prefab, coin_good_prefab, coin_ura_prefab;

    public void SetPlaceId(int p, int w, int h)
    {
        this.panel_id  = p;
        this.width_id  = w;
        this.height_id = h;
    }

    public void UpdateCoinPlace(int coin_id)
    {
        switch (coin_id)
        {
            case 0: DeleteCoin();  break;
            case 1: SetCoinEvil(); break;
            case 2: SetCoinGood(); break;
            case 3: SetCoinUra();  break;
            default: break;
        }
    }

    public void SetCoinEvil()
    {
        DeleteCoin();
        GameObject go = Instantiate(coin_evil_prefab, transform);
        go.name = "coin_evil_clone";
        coin_id = 1;
    }

    public void SetCoinGood()
    {
        DeleteCoin();
        GameObject go = Instantiate(coin_good_prefab, transform);
        go.name = "coin_good_clone";
        coin_id = 2;
    }

    public void SetCoinUra()
    {
        DeleteCoin();
        GameObject go = Instantiate(coin_ura_prefab, transform);
        go.name = "coin_ura_clone";
        coin_id = 3;
    }

    public void DeleteCoin()
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        coin_id = 0;
    }

    public void OnMouseDown()
    {
        GameObject go = GameObject.Find("geister_director");
        go.gameObject.GetComponent<GeisterDirector>().SetCoinPlace( gameObject );
    }

    /// <summary>
    /// 駒の状態を返す
    /// </summary>
    /// <returns></returns>
    public string Kind()
    {
        switch (coin_id)
        {
            case 0: return "";
            case 1: return "evil";
            case 2: return "good";
            case 3: return "unknown";
        }

        return "";
    }

    public int CoinId(string kind)
    {
        switch (kind)
        {
            case "":        return 0;
            case "evil":    return 1;
            case "good":    return 2;
            case "unknown": return 3;
        }

        return 0;
    }
}
