using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}