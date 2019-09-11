using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinPlaceGenerator : MonoBehaviour {

    public GameObject coin_panel;
    public GameObject coin_place;

    public void CreateCoinPlace(int width, int height)
    {
        for (int i = 1; i <= width; i++)
        {
            for (int j = 1; j <= height; j++)
            {
                GameObject go = Instantiate(coin_place, coin_panel.transform);
                go.transform.position = new Vector3(i * 80 - 40, j * 80 - 40, 0);
            }
        }
    }
}
