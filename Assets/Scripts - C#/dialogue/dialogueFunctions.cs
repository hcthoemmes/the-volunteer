using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Yarn.Unity;

public class dialogueFunctions : MonoBehaviour
{
    
    [YarnCommand("begin_dialogue")]
    // This command should not be necessary
    // Actually, this whole function probably isn't
    // Keeping it to be safe though
    public void startDialogue(dialogueInfo d)
    {
        dialogueSingleton.Instance.loadDialogue(d, this);
    }

    [YarnCommand("end_dialogue")]
    public static void endDialogue()
    {
        dialogueSingleton.Instance.endDialogue();
    }

    
    [YarnCommand("load_sprite")]
    public static void loadSprite(string direction, string name)
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
