using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InventoryUIManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField]
    private CharacterScriptableObject player;
    
    [SerializeField] 
    private int playerLives = 3;
    
    public CharacterImpl Player => player.Character as CharacterImpl;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            StartCoroutine(HandlePlayerDeath());
        }
        else
        {
            StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        yield return new WaitForSeconds(2);
        TakeLife();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(2);
        // TODO: implement and show game over screen
        ResetGameSession();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        playerLives--;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
