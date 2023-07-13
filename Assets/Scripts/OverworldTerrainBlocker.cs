using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldTerrainBlocker : MonoBehaviour
{

    public int minRootedLevel = 2; // Required rooted level to be able to traverse
    public OverworldPlayerController controller;
    
    void Start()
    {
        if(minRootedLevel <= 0 || controller.GetRootednessLevel() >= minRootedLevel)
        {
            Destroy(gameObject); // Destroy self if rootedness level is greater than minimum required
        }
    }

}
