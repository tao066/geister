using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPanelController : MonoBehaviour {

    public GameObject coin_place_prefab;

    public void CreateCoinPlace(int panel_id)
    {
        int width  = 8;
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

    public void InitialiseSetCoin()
    {
        foreach (Transform coin_place in transform)
        {
            CoinPlaceController coin_place_controller = coin_place.gameObject.GetComponent<CoinPlaceController>();

            coin_place_controller.SetCoinUra();
        }
    }
}
