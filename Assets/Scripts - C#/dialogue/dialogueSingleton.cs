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

    private dialogueSingleton() { }

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

    public Dictionary<string, characterGeneric> charsInScene = new();

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

            // Set it up such that we have chars by their name
            foreach (characterGeneric c in d.sceneChars)
            { charsInScene.Add(c.charName, c); }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("dialogueGeneric"));
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);

            // This may be redundant or otherwise unnecessary
            yield return m.StartCoroutine(loadAssets(d));

            DialogueRunner dR;
            dR = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
            
            dR.Stop(); // Cancel the autoloaded dialogue. This feels cheap... but works.
            dR.StartDialogue(d.nodeName);
            //In calling up the dialogue with the named node, the Yarn script takes over at this point.
            // Some of this could probably be put in a dialogueBegin function, or something

            // loadLeftSprite(d);

        aO.allowSceneActivation = true;

        Debug.Log("Dialogue scene loaded");

        // Set active
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName("dialogueGeneric"));
    }



    // --------------------------------------------------------------------------------------------------- //
    //     This part of the code is all the fun ways to load in a sprite. These fun ways are called in a   //
    //  chain later on by dialogueFunctions. Anyway, you don't need to worry about this spot when writing  //
    //                                dialogue. Here be dragons!                                           //
    // --------------------------------------------------------------------------------------------------- //

    public dialogueSingleton loadSprite(string name, string direction)
    {
        // Load, but do not display, the sprite.
        Sprite s;
        characterGeneric c;

        charsInScene.TryGetValue(name, out c);
        s = c.talkSprite;

        // One of these sides needs to flip the sprite along the X-axis.
        if (direction == "left") {
            SpriteRenderer sL = findRenderSide("leftSprite");
            sL.enabled = false;
            sL.sprite = s;
            Debug.Log("Left sprite loaded.");
        } 
        
        else if (direction == "right") {
            SpriteRenderer sR = findRenderSide("rightSprite");
            sR.enabled = false;
            sR.sprite = s;
            Debug.Log("Right sprite loaded.");
        } 
        
        // The if/else could probably be removed entirely by making findRenderSide look for d+"Sprite"

        else {
            Debug.Log("Invalid direction entered");
        }

        return Instance;
    }

    public dialogueSingleton makeVisibleSprite(string side)
    {
        SpriteRenderer sr = findRenderSide(side+"Sprite");
        sr.enabled = true;
        return Instance;
    }

    public dialogueSingleton makeInvisibleSprite(string side)
    {
        SpriteRenderer sr = findRenderSide(side+"Sprite");
        sr.enabled = false;
        return Instance;
    }

    SpriteRenderer findRenderSide(string d) { return GameObject.Find(d).GetComponent<SpriteRenderer>(); }

    IEnumerator loadAssets(dialogueInfo d)
    {


        // Set node
        Debug.Log("Assets loaded!");
        yield return null;
    }

}
