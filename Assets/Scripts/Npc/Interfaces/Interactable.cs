using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    /*
     * Called when we want to interact with an NPC.
     */
    public bool interact(bool dialogueIsOpen);
}
