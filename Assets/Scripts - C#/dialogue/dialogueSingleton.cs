using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

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

    // Also - if a thing accepts characterGeneric, it'll accept the children of
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

    // MonoBehaviour taken as a parameter to use coroutines
    public void loadDialogue(dialogueInfo d, MonoBehaviour mono)
    {
        if(dialogueState == 0)
        {
            mono.StartCoroutine(loadScene(mono, d));
            Debug.Log("Dialogue loading.");
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

    void loadCharacter()
    {

    }

    IEnumerator monoBorrowTest()
    {
        Debug.Log("Test running!");
        yield return new WaitForSeconds(3f);
        Debug.Log("Closing now!");
        yield return null;
    }

    // Asynchronously load dialogue scene, wait for completion
    IEnumerator loadScene(MonoBehaviour m, dialogueInfo d)
    {
        AsyncOperation aO = SceneManager.LoadSceneAsync("dialogueGeneric", LoadSceneMode.Additive);

        while (!aO.isDone)
        { yield return null; }

        // Once load is complete, load assets into scene
        aO.allowSceneActivation = false;

            // Todo: Unload, and then later reload, the EventSystem in the scene that called this. Not necessary, but
            // Unity keeps throwing a lot of complaints.

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("dialogueGeneric"));
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);

            // This may be redundant
            yield return m.StartCoroutine(loadAssets(d));

            DialogueRunner dR;
            dR = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
            dR.Stop(); // Cancel the autoloaded dialogue. This feels cheap... but works.
            dR.StartDialogue(d.nodeName);

            // JUST TESTING: Loading up sprites.
            SpriteRenderer sL = GameObject.Find("leftSprite").GetComponent<SpriteRenderer>();
            sL.sprite = d.sceneChars[0].talkSprite;

        aO.allowSceneActivation = true;

        Debug.Log("Dialogue scene loaded");

        // Set active
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName("dialogueGeneric"));
    }

    IEnumerator loadAssets(dialogueInfo d)
    {


        // Set node
        Debug.Log("Assets loaded!");
        yield return null;
    }

}
