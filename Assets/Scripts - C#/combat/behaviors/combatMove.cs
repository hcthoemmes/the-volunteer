using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(menuName = "Move")]
public class combatMove : combatBehavior
{
    // These might not be necessary, even
    targetingMode mode = targetingMode.lineOfSight;
    targetingType type = targetingType.point;

    public float moveSpeed = 1f;

    // Pass in the monoBehaviour to handle gameobjects
    public override void run(MonoBehaviour m)
    {
        if (m.gameObject.transform != targetLocation)
        {
            // Move some amount closer to the thing
        }
    }

    public override void findTarget()
    {
        /*
         * if there's already a targetLocation, skip this bit
         * otherwise, 
         * 
         */

        Debug.Log("Finding target!");
    }



    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }
}
