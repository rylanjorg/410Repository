using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;

    public TimerController timerController;

    public TMP_Text coinText;
    public int currentCoins = 0;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        coinText.text = "Coins : " + currentCoins.ToString() + "/10";
    }
    void Update()
    {
        if (currentCoins == 10)
        {
            timerController.StopTimer();
        }
    }

    public void IncreaseCoins(int v)
    {
        currentCoins += v;
        coinText.text = "Coins : " + currentCoins.ToString() + "/10";
    }
}
