using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface QuestInterface
{
    public void handleQuest();

    public bool isQuestActive();

    public void setQuestAsActive();

    public bool isQuestComplete();

    public bool canHandInQuest();

    public void setQuestDetails(string details);

    public string getQuestDetails();

    public void updateUI();
}
