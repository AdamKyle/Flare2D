using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public int letterSpeed;

    // Empty game object for the dialogue box.
    public GameObject dialogueBox;

    // Store the text for the dialogue.
    public Text dialogueText;

    private List<int> dialogueIndexes;

    private int currentIndex;

    private bool shouldClearDialogueBox;

    // Allows us to reference this class through doing DialogueManager.dialogueManager.methodName()
    // This makes the class a static singleton.
    public static DialogueManager dialogueManager { get; private set; }

    public void Awake()
    {
        dialogueManager = this;

        dialogueIndexes = new List<int>();

        shouldClearDialogueBox = false;
    }

    // Show the dialogue box.
    public void showDialogue(Dialogue dialogue, bool dialogueIsOpen)
    {
        dialogueBox.SetActive(true);

        if (dialogueIsOpen && !dialogueIndexes.Any()) {
            StartCoroutine(typeText(dialogue.LinesOfText[0]));

            dialogueIndexes.Add(0);
        } else if (dialogueIsOpen && dialogueIndexes.Any()) {
            currentIndex = dialogueIndexes.Last();

            currentIndex += 1;

            if (dialogue.LinesOfText.ElementAtOrDefault(currentIndex) != null) {
                StartCoroutine(typeText(dialogue.LinesOfText[currentIndex]));

                dialogueIndexes.Add(currentIndex);

                currentIndex += 1;

                if (dialogue.LinesOfText.ElementAtOrDefault(currentIndex) == null) {
                    shouldClearDialogueBox = true;
                }
            }
        }
    }

    public bool doneTalking() {
        return shouldClearDialogueBox;
    }

    public void cleanUp() {
        dialogueIndexes.Clear();

        dialogueBox.SetActive(false);
    }

    // Types out the text letter by letter based on the speed of each letter.
    private IEnumerator typeText(string dialogueString) {
        dialogueText.text = "";

        foreach (var letter in dialogueString) {

            dialogueText.text += letter;

            yield return new WaitForSeconds(1f / letterSpeed);
        }
    }
}
