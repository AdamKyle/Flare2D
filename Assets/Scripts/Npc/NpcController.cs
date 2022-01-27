using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour, Interactable
{

    [SerializeField] Dialogue dialogue;

    [SerializeField] Dialogue questDialogue;

    private CoinCollector coinCollectorQuest;

    public bool interact(bool dialogueIsOpen)
    {
        coinCollectorQuest = gameObject.AddComponent<CoinCollector>();

        if (coinCollectorQuest.hasCoins()) {
            if (DialogueManager.dialogueManager.doneTalking()) {
                DialogueManager.dialogueManager.cleanUp();

                coinCollectorQuest.processCoins();

                return false;
            } else {
                DialogueManager.dialogueManager.showDialogue(questDialogue, dialogueIsOpen);

                return true;
            }
        } else {
            if (DialogueManager.dialogueManager.doneTalking()) {
                DialogueManager.dialogueManager.cleanUp();

                return false;
            } else {
                DialogueManager.dialogueManager.showDialogue(dialogue, dialogueIsOpen);

                return true;
            }
        }
    }
}
