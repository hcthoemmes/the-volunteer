using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTestListener : MonoBehaviour
{
    // This listener thing is not how the game is going to work.
    // We're just doing this to have a simple way to test loading
    // characters into the dialogueGeneric scene.

    [SerializeField] private dialogueInfo a;
    [SerializeField] private dialogueInfo b;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("z"))
        {
            dialogueSingleton.Instance.loadDialogue(a, this);
        }

        if (Input.GetKeyDown("x"))
        {
            dialogueSingleton.Instance.loadDialogue(b, this);
        }

        if (Input.GetKeyDown("c"))
        {
            dialogueSingleton.Instance.endDialogue();            
        }
    }

    IEnumerator waiter(float time)
    {
        dialogueSingleton.Instance.loadDialogue(a, this);
        // this is a debug coroutine. It waits
        yield return new WaitForSeconds(time);
        dialogueSingleton.Instance.endDialogue();
        yield return null;
    }

}
