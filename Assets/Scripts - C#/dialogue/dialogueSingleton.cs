using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The artist FKA grodyDialogueManagerSingleton

// Of note: CANNOT inherit from monobehaviour, at least in this form! It's a no-no!
// For whatever reason, it won't do the thing called "being a singleton" if it does
public sealed class dialogueSingleton
{
    private static dialogueSingleton instance = null;
    private int dialogueState = 0; // 0 for not in dialogue, 1 for in dialogue
    private static readonly object padlock = new object();

    private dialogueSingleton() {
    
    }

    /*
     * This class will handle the functional parts of loading dialogue:
     * 
     */

    // Also - if it accepts characterGeneric, it'll accept the children of
    // characterGeneric too. That makes that easy!

    public static dialogueSingleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new dialogueSingleton();
                }
                return instance;
            }
        }
    }

    /* Maybe on an enter event? We get all the characters in the scene, and then we
     * put a knife in their gullet. No, we actually just load them into an array
    */
    public void loadDialogue(dialogueInfo d)
    {
        if(dialogueState == 0)
        {
            SceneManager.LoadScene("dialogueGeneric", LoadSceneMode.Additive);
            Debug.Log("Dialogue loaded.");
            dialogueState = 1;
        }
        else
        {
            Debug.Log("Dialogue currently loaded.");
        }

        return;
    }

    public void endDialogue()
    {
        if (dialogueState == 1)
        {
            SceneManager.UnloadSceneAsync("dialogueGeneric");
            Debug.Log("Dialogue unloaded.");
            dialogueState = 0;
        }
        else
        {
            Debug.Log("Dialogue currently unloaded.");
        }

        return;
    }

    public void testFunction()
    {
        Debug.Log("Singleton test called.");
    }
}
