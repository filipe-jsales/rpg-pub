using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //[SerializeField] float levelLoadDelay = 1f;
    [SerializeField] int playerLives = 3;
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
    public void ProcessPlayerDamageTaken()
    {
        if (playerLives > 1)
        {
            TakeLife();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            ResetGameSession();
        }
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        //TODO: change the logic to no longer reset the level, but instead add an animation of the player taking damage and being invincible for a few seconds
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
