using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Combat Behavior")]
public abstract class combatBehavior : ScriptableObject
{
    // Amount of time to exist
    // Targeting type - point, 
    // [Header("Targeting")]

    protected Transform targetLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void findTarget();

    public abstract void run(MonoBehaviour m); // Overridable

    public enum targetingType { circle, line, cone, point }
    public enum targetingMode { onPlayer, nearPlayer, lineOfSight, random }
    // LoS mode for movement action - basically, that one will be a point targeted by LoS
    public enum objectToTarget { player, random, objective }
    // Objective target to say "hey! 

}

