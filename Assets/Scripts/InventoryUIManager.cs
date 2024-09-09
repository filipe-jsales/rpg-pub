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
        var character = _gameManager.Player;
        var images = inventoryCanvas.GetComponentsInChildren<Image>();
        var inventorySlots =  images.Where(i => i.gameObject.name.Contains("InventorySlot")).ToArray();
        var equippedItems = images.Where(i => i.gameObject.name.Contains("Equipped")).ToArray();

        foreach (var equipped in equippedItems)
        {
            switch (equipped.gameObject.name)
            {
                case "EquippedWeapon":
                    equipped.sprite = character.EquippedWeapon.Sprite;
                    break;
                case "EquippedArmor":
                    equipped.sprite = character.EquippedArmor.Sprite;
                    break;
            }
        }
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            var image = inventorySlots[i];
            var item = items[i];
            image.sprite = item.Sprite;
            if (image.gameObject.GetComponent<Button>() == null)
            {
                var button = image.gameObject.AddComponent<Button>();
                button.onClick.AddListener(() => item.OnInteract.Invoke());
            }
            
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
}
