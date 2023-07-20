using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldAreaPoint : MonoBehaviour
{
    public string levelToLoad = "";
    public GameObject nameDisplayer;

    private bool canEnterLevel = false;

    OverworldPlayerController playerController;

    [SerializeField] private AK.Wwise.Event playerCollideSound;

    private void Start()
    {
        playerController = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<OverworldPlayerController>() != null)
        {
            playerController = collision.gameObject.GetComponent<OverworldPlayerController>();
            canEnterLevel = true;
            playerCollideSound.Post(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<OverworldPlayerController>() != null)
        {
            canEnterLevel = false;
        }
    }

    private void Update()
    {
        if (canEnterLevel && playerController != null)
        {
            nameDisplayer.SetActive(true);
            if (Input.GetKeyDown(playerController.GetActionKey()))
            {
                SceneManager.LoadScene(levelToLoad); // Can add extra things like loading screen, etc.
            }
        } else
        {
            nameDisplayer.SetActive(false);
        }
    }
}
