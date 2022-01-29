using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour, QuestInterface
{

    private bool isComplete;

    private bool isActive;

    private string questDetails;

    public bool isQuestComplete() {
        return isComplete;
    }

    private bool hasCoins()
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

        Text coinsText = canvas.GetComponent<Text>();

        coinsText.text = "";

        isComplete = true;

        this.updateQuestLog();

        this.setQuestAsInActive();
    }

    public void handleQuest() {
        this.processCoins();
    }

    public bool isQuestActive() {
        return isActive;
    }

    public void setQuestAsActive() {
        isActive = true;

        GameObject canvas = GameObject.Find("Coins");

        Text coinsText = canvas.GetComponent<Text>();

        coinsText.text = "Coins: 0";
    }

    public void setQuestAsInActive() {
        isActive = false;
    }

    public void setQuestDetails(string details) {
        questDetails = details;
    }

    public string getQuestDetails() {
        return questDetails;
    }

    public void updateUI() {
        GameObject canvas = GameObject.FindWithTag("Coin UI");

        Text coinsText = canvas.GetComponent<Text>();

        coinsText.text = "Coins: " + CoinCollection.totalCoins;
    }

    public bool canHandInQuest() {
        return this.hasCoins();
    }

    private void updateQuestLog() {
        QuestLog.questLogEntries["Coin Collection"] = this;
    }
}
