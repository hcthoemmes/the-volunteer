using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this to a GameObject, make a prefab, and then instantiate the prefab
public class combatInteractable : MonoBehaviour
{

    SpriteRenderer sR;

    [Header("GameObject Fields")]
    public Sprite sprite;
    public Collider2D cD;

    [Header("Positioning and Timing")]
    public float x;
    public float y;
    public float respawnDelay;
    public float actionDelay;

    [Header("Combat Specifics")]
    public float health = 1;
    public combatBehavior[] behaviors;

    private void Awake()
    {
        sR = this.GetComponent<SpriteRenderer>();
        cD = this.GetComponent<Collider2D>();
        sR.sprite = sprite;
    }
        
    // Update is called once per frame
    void Update()
    {
        

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        // In the combat manager, see how many combatInteractables are in the scene
        // If there's fewer than expected, wait for the minimum delay + a random time amount, then spawn more
    }

    void doBehavior(combatBehavior b)
    {
        // For instance, for an apple rotting, reduce health by some amount until destroyed
        b.run(this);
    }

}
