using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class can be added to to handle items, quests and so on.
[System.Serializable]
public class Dialogue
{

    // Store the lines of text.
    public List<string> linesOfText;

    // Allows us to get the lines of text.
    public List<string> LinesOfText
    {
        get { return linesOfText; }
    }
}
