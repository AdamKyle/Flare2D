using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{

    private bool isComplete;

    public bool isQuestComplete() {
        return isComplete;
    }

    public bool hasCoins()
    {
        if (CoinCollection.totalCoins >= 12)
        {
            return true;
        }

        return false;
    }

    public void processCoins()
    {
        CoinCollection.totalCoins = 0;

        GameObject canvas = GameObject.Find("Coins");

        Debug.Log(canvas);

        Text coinsText = canvas.GetComponent<Text>();

        Debug.Log(coinsText);

        coinsText.text = "";

        isComplete = true;
    }
}
