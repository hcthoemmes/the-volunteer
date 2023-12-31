using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is intended to be the base class for any
 * "characters" we implement - in the current case, that
 * means characters in the non-overworld areas.
 * 
 * Any characters should derive from this class.
 */

[CreateAssetMenu(menuName ="Create Generic Character")]
public class characterGeneric : ScriptableObject
{
    // Only definitions here, probably
    public string charName;
    public Sprite talkSprite, areaSprite;
}
