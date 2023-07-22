using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Yarn.Unity;

public class dialogueFunctions : MonoBehaviour
{

    // This command should not be necessary and in fact, would not work. We're keeping it in for now anyway
    [YarnCommand("begin_dialogue")]
    public void startDialogue(dialogueInfo d)
    {
        dialogueSingleton
            .Instance
            .loadDialogue(d, this);
    }

    [YarnCommand("end_dialogue")]
    public static void endDialogue()
    {
        dialogueSingleton
            .Instance
            .endDialogue();
    }


    // FROM HERE BELOW begin the "load and do something pretty" functions. Here be cyclopes.

    [YarnCommand("load_and_show")]
    public static void loadAndShow(string n, string d)
    {
        dialogueSingleton
            .Instance
            .loadSprite(n, d)
            .makeVisibleSprite(d);
    }

    [YarnCommand("make_invisible")]
    public static void makeInvisible(string d)
    {
        dialogueSingleton
            .Instance
            .makeInvisibleSprite(d);
    }

    //public static void (string name, string direction)
    // dialogueSingleton.Instance()
        //.loadSprite ("name", "direction")
            // loadSprite has to find a sprite by name
        //.doPrettiness()
        //.showSprite

}