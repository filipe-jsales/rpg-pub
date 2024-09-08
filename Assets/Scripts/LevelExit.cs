using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] 
    private float levelLoadDelay = 1f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
            //LoadNextLevel();
        }
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("End of game reached! Restarting...");
            RestartLevel();
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

}
