using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTestListener : MonoBehaviour
{
    // This listener thing is not how the game is going to work.
    // We're just doing this to have a simple way to test loading
    // characters into the dialogueGeneric scene.

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("z"))
        {
            characterGeneric hatChar = GameObject.Find("hatPerson").GetComponent<characterGeneric>();
            characterGeneric[] usedChars = new characterGeneric[] { hatChar };
            dialogueFunctions.startDialogue(usedChars);
        }

        if (Input.GetKeyDown("x"))
        {

        }

    }
}
