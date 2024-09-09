using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas inventoryCanvas;
    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryCanvas.gameObject.SetActive(!inventoryCanvas.gameObject.activeSelf);
        }
        if (inventoryCanvas.gameObject.activeSelf)
        {
            StartCoroutine(UpdateInventoryUI());
        }
        
    }

    private IEnumerator UpdateInventoryUI()
    {
        UpdateInventorySlotImages();
        UpdateTextComponents();
        yield return new WaitForSeconds(2f);
    }

    private void UpdateInventorySlotImages()
    {
        var items = _gameManager.Items;
        var images = inventoryCanvas.GetComponentsInChildren<Image>();
        images = images.Where(i => i.gameObject.name.Contains("InventorySlot")).ToArray();
        
        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = items[i].Sprite;
        }
    }

    private void UpdateTextComponents()
    {
        var character = _gameManager.Player;
        var textComponents = inventoryCanvas.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textComponent in textComponents)
        {
            var text = textComponent.text.Split(":")[0];
            switch (text)
            {
                case "Name":
                    textComponent.text = text +": " + character.Name;
                    break;
                case "Health":
                    textComponent.text = text +": " + character.Health;
                    break;
                case "Base Damage":
                    textComponent.text = text +": " + character.BaseDamage;
                    break;
                case "Base Poise":
                    textComponent.text = text +": " + character.BasePoise;
                    break;
                case "Weapon Name":
                    textComponent.text = text +": " + character.EquippedWeapon.Name;
                    break;
                case "Armor Name":
                    textComponent.text = text +": " + character.EquippedArmor.Name;
                    break;
            }
        }
    }
    
    public void SwitchToAnotherRandomWeapon()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().SwitchToAnotherRandomWeapon();
    }
}
