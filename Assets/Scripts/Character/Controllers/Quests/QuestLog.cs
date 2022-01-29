using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    public static Dictionary<string, QuestInterface> questLogEntries;

    public static QuestLog questLog { get; private set; }

    public void Awake() {

        DontDestroyOnLoad(this);

        questLog = this;

        if (questLogEntries == null) {
            questLogEntries = new Dictionary<string, QuestInterface>();
        } else {
            foreach (var quest in questLogEntries) {
                if (!quest.Value.isQuestComplete() && quest.Value.isQuestActive()) {
                    Debug.Log("Updating Quest UI");
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
}
