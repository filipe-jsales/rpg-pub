using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
            StartCoroutine(HandlePlayerDamage());
        }
        else
        {
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private IEnumerator HandlePlayerDamage()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator HandlePlayerDeath()
    {
        yield return new WaitForSeconds(2);

        ResetGameSession();
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void TakeLife()
    {
        playerLives--;
        Debug.Log("Vida restante: " + playerLives);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
