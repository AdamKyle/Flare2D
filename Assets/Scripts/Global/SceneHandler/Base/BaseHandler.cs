using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHandler : MonoBehaviour
{

    public List<GameObject> coins;

    // Start is called before the first frame update
    void Start()
    {
        this.hideOrShowCoins();
    }

    // Update is called once per frame
    void Update()
    {
        this.hideOrShowCoins();
    }

    protected void hideOrShowCoins() {
        if (QuestLog.questLog.hasQuest("Coin Collection")) {
            QuestInterface npcQuest = QuestLog.questLog.quest("Coin Collection");

            if (npcQuest.isQuestActive()) {
                this.updateCoinVisibility(true);
            } else {
                this.updateCoinVisibility(false);
            }
        } else {
            this.updateCoinVisibility(false);
        }
    }

    private void updateCoinVisibility(bool visible) {
        foreach (GameObject gameObject in coins) {
            if (gameObject != null) {
                gameObject.SetActive(visible);
            }
        }
    }

}
