using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InventoryUIManager))]
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField]
    private CharacterScriptableObject player;
    
    [SerializeField] 
    private int playerLives = 3;
    
    private AudioSource _audioSource;
    
    public CharacterImpl Player => player.Character as CharacterImpl;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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

    public void PlayClip(AudioClip clip)
    {
        FadeOut();
        _audioSource.PlayOneShot(clip);
    }
    
    public float fadeDuration = 1f; // Adjust the fade duration as needed

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        float startVolume = _audioSource.volume;

        while (elapsedTime < fadeDuration)
        {
            _audioSource.volume = Mathf.Lerp(startVolume, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = 1f;
    }
    
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        float startVolume = _audioSource.volume;

        while (elapsedTime < fadeDuration)
        {
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = 0f;
        _audioSource.Stop(); // Stop the audio source after fading out
        _audioSource.volume = startVolume;
    }

    private IEnumerator HandlePlayerDeath()
    {
        yield return new WaitForSeconds(2);
        // TODO: reset player health on death
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
