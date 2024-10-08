using System.Collections;
using System.Collections.Generic;
using Abstractions;
using Impl;
using Interfaces;
using ScriptableObjects;
using TMPro;
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
    private int score = 0;

    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI livesText;
    [SerializeField]
    TextMeshProUGUI scoreText;

    public CharacterImpl Player => player.Character as CharacterImpl;
    public Character Character => player.Character;
    public List<IRpgObject> Items => player.Items;
    
    private InventoryUIManager _inventoryUIManager;

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

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
        _inventoryUIManager = GetComponent<InventoryUIManager>();
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
        livesText.text = playerLives.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddToScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void ShowItemDescription(IRpgObject item, Transform itemTransform)
    {
        _inventoryUIManager.ShowItemDescription(item, itemTransform);
    }
    
    public void DestroyCurrentItemDescription()
    {
        _inventoryUIManager.DestroyCurrentItemDescription();
    }

    public void UpdateInventoryUI()
    {
        _inventoryUIManager.UpdateUI();
    }
}
