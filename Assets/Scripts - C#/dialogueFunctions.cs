using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class dialogueFunctions : MonoBehaviour
{

    // An array is used here so we might later do more than two characters
    // talking simultaneously

    // Also, this is just for me - we use static so we don't need to make an instance
    // of dialogueFunctions. We wanna use this script like a header
    public static void startDialogue(characterGeneric[] c)
    {
        SceneManager.LoadSceneAsync("dialogueGeneric", LoadSceneMode.Additive);
        GameObject sp1 = GameObject.Find("Sprite1");
        SpriteRenderer sr = sp1.GetComponent<SpriteRenderer>();

        sr.sprite = c[0].talkSprite;

        // Load dialogueGeneric scene
        // Turn down the gamma, or otherwise darken, the scene that was loaded from
        // get the characters from c and load their talkSprites into the scene
    }

    // we should also add a showCharacter function

    public static void endDialogue()
    {

    }

}


/* TO DO: 
 *  Add a changeCharacter Yarn function.
 *  General format:
 *      [YarnCommand("change_character")]
 *      public void changeCharacter(int side, int index, transition t = none)
 *      {
 *          - A comment in the comment: this could be 
 *           written such that index is a string for the name.
 *           Would be more easily readable. Also side could be
 *           a single bit, or something smaller than an int.
 *           
 *           switch(side){
 *              case 0:
 *                  // perform transition
 *                  // attach character by index to left side gameobject
 *                  // break
 *              case 1:
 *                  // perform transition
 *                  // attach character by index to right side gameobject
 *                  // break
 *           }
 *          
 *       }
 *       
 *       
 *  Add a transition base class 
 */
