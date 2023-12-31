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

    public GameObject loadingScreen;

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
                
                StartCoroutine(LoadingLevel(levelToLoad));
            }
        } else
        {
            nameDisplayer.SetActive(false);
        }
    }

    IEnumerator LoadingLevel(string name)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2.1f);
        SceneManager.LoadScene(name); // Can add extra things like loading screen, etc.
    }
}
