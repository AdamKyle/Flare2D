using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    public static Dictionary<string, QuestInterface> questLogEntries;

    public static QuestLog questLog { get; private set; }

    public GameObject questLogUI;

    public void Awake() {

        DontDestroyOnLoad(this);

        questLog = this;

        if (questLogEntries == null) {
            questLogEntries = new Dictionary<string, QuestInterface>();
        } else {
            foreach (var quest in questLogEntries) {
                if (!quest.Value.isQuestComplete() && quest.Value.isQuestActive()) {
   
                    quest.Value.updateUI();
                }
            }
        }
    }

    public Dictionary<string,QuestInterface> logEntries() {
        return questLogEntries;
    }

    public bool hasQuest(string name) {
        if (questLogEntries.ContainsKey(name)) {
            return true;
        }

        return false;
    }

    public QuestInterface quest(string name) {
        return questLogEntries[name];
    }

    public bool isQuestActive(string name) {
        if (this.quest(name) != null) {
            return this.quest(name).isQuestActive();
        }

        return false;
    }

    public bool isQuestComplete(string name) {
        if (this.quest(name) != null) {
            return this.quest(name).isQuestComplete();
        }

        return false;
    }

    public void manageQuestLogUi(bool active) {
        questLogUI.SetActive(active);

        if (active) {
            this.updateQuestLog();
        } else {
            GameObject contentSection = questLogUI.transform.GetChild(0).gameObject;

            foreach (Transform child in contentSection.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void updateQuestLog() {
        if (questLogEntries.Any()) {
            GameObject contentSection = questLogUI.transform.GetChild(0).gameObject;

            foreach (var questEntry in questLogEntries) {
                if (!questEntry.Value.isQuestComplete()) {
                    
                    GameObject questLogUiText = new GameObject();

                    questLogUiText.transform.SetParent(contentSection.transform, false);
                    
                    Text questText = questLogUiText.AddComponent<Text>();
                    
                    questText.text = questEntry.Value.getQuestDetails();

                    Font arialFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

                    questText.font = arialFont;
                    questText.color = new Color(0.45f,0.45f,0.45f, 1.0f);
                }
            }
        }
    }
}
