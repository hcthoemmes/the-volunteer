using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The artist FKA grodyDialogueManagerSingleton
public sealed class dialogueSingleton : MonoBehaviour
{
    private static dialogueSingleton instance = null;
    private static readonly object padlock = new object();
    dialogueSingleton() { }

    /*
     * This one's a gross sort of hack, but we're using it cause
     * it's a nice, easy way of doin global stuff!
     * 
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
                if(instance == null)
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
    void loadDialogue(characterGeneric[] sceneChars)
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
