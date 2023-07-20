using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holder class for easier loading
[CreateAssetMenu(menuName = "Dialogue Information")]
public class dialogueInfo : ScriptableObject
{
    public string nodeName;
    public characterGeneric[] sceneChars;
}
