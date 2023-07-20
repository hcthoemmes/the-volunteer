using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTestListener : MonoBehaviour
{
    // This listener thing is not how the game is going to work.
    // We're just doing this to have a simple way to test loading
    // characters into the dialogueGeneric scene.

    [Header("Dialogue to Load")]
    [SerializeField] private dialogueInfo a;
    [SerializeField] private dialogueInfo b;
    [SerializeField] private dialogueInfo c;

    [Header("Noises")]
    [SerializeField] private AK.Wwise.Event pressSound;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("z"))
        {
            dialogueSingleton.Instance.loadDialogue(a, this);
            pressSound.Post(gameObject);
        }

        if (Input.GetKeyDown("x"))
        {
            dialogueSingleton.Instance.loadDialogue(b, this);
            pressSound.Post(gameObject);
        }

        if (Input.GetKeyDown("a"))
        {
            dialogueSingleton.Instance.loadDialogue(c, this);
            pressSound.Post(gameObject);
        }

        if (Input.GetKeyDown("c"))
        {
            dialogueSingleton.Instance.endDialogue();
            pressSound.Post(gameObject);
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
