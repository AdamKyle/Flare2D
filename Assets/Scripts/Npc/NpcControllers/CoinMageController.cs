using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMageController : MonoBehaviour, Interactable
{

    [SerializeField] Dialogue dialogue;

    [SerializeField] Dialogue questDialogue;

    [SerializeField] Dialogue questNotCompleteDialogue;

    public GameObject questIcon;

    public GameObject questCompleteIcon;

    public Camera gameCamera;

    private QuestInterface npcQuest;

    private CoinCollector coinCollectorQuest;

    public void Start() {
        if (!QuestLog.questLog.hasQuest("Coin Collection")) {
            questIcon.SetActive(true);
        }
    }

    public void Update() {
        if (QuestLog.questLog.hasQuest("Coin Collection")) {
            questIcon.SetActive(false);

            npcQuest = QuestLog.questLog.quest("Coin Collection");

            if (!npcQuest.isQuestComplete() && npcQuest.canHandInQuest()) {
                questCompleteIcon.SetActive(true);
            }

            if (npcQuest.isQuestComplete()) {
                questCompleteIcon.SetActive(false);
            }
        }
    }

    public bool interact(bool dialogueIsOpen) {

        gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, transform.position, Time.deltaTime);

        if (QuestLog.questLog.hasQuest("Coin Collection")) {
            npcQuest = QuestLog.questLog.quest("Coin Collection");

            if (npcQuest.canHandInQuest() && !npcQuest.isQuestComplete()) {
                return this.finishQuest(questDialogue, npcQuest, dialogueIsOpen);
            }

            if (!npcQuest.isQuestComplete()) {
                return this.chatWithNpc(questNotCompleteDialogue, dialogueIsOpen);
            }

            if (npcQuest.isQuestComplete()) {
                return false;
            }
        }

        return this.startQuest(dialogue, dialogueIsOpen);
    }

    private void addQuestToLog() {
        coinCollectorQuest = gameObject.AddComponent<CoinCollector>();

        coinCollectorQuest.setQuestDetails("The mysterious lady needs her coins! Help to find all twelve by exploring the area and collecting coins. These will appear at the top left. Once you have all twelve, go back to her.");

        coinCollectorQuest.setQuestAsActive();

        QuestLog.questLogEntries.Add("Coin Collection", coinCollectorQuest);
    }

    private bool chatWithNpc(Dialogue dialogue, bool isOpen) {
        return DialogueManager.dialogueManager.showDialogue(dialogue, isOpen);
    }

    private bool startQuest(Dialogue dialogue, bool isOpen) {
        bool talking = DialogueManager.dialogueManager.showDialogue(dialogue, isOpen); 

        if (!talking) {
            this.addQuestToLog();

            return false;
        }

        return talking;
    }

    private bool finishQuest(Dialogue dialogue, QuestInterface quest, bool isOpen) {
        bool talking = DialogueManager.dialogueManager.showDialogue(dialogue, isOpen);

        if (!talking) {
            quest.handleQuest();

            return false;
        }

        return talking;
    }
}
