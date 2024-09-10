using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas inventoryCanvas;
    
    [SerializeField]
    private Transform inventorySlotCanvas;
    
    [SerializeField]
    private GameObject inventorySlotPrefab;
    
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
        var equippedItems = images.Where(i => i.gameObject.name.Contains("Equipped")).ToArray();
        var existingSlots = inventorySlotCanvas.gameObject.GetComponentsInChildren<Image>()
            .Select(i => i.gameObject.name)
            .ToArray();

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

        foreach (var item in items)
        {
            var componentName = "Inventory Slot - " + item.Name;
            // TODO: se for tiver vários do mesmo item, essa lógica vai impedir de aparecer na UI
            if (existingSlots.Contains(componentName))
            {
                continue;
            }
            
            var slotComponent = Instantiate(inventorySlotPrefab, inventorySlotCanvas);
            slotComponent.name = componentName;
            var slotImage = slotComponent.GetComponent<Image>();
            slotImage.sprite = item.Sprite;
            if (slotComponent.GetComponent<Button>() == null)
            {
                var button = slotComponent.AddComponent<Button>();
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
