using System.Collections.Generic;
using System.Linq;
using Impl;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SortByItem
{
    Armor, Weapon, Obtained
}

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas inventoryCanvas;
    
    [SerializeField]
    private Transform inventorySlotCanvas;
    
    [SerializeField]
    private GameObject inventorySlotPrefab;
    
    [SerializeField]
    private GameObject itemDescriptionPrefab;
    
    private GameManager _gameManager;
    
    private SortByItem _sortByItem = SortByItem.Obtained;
    private GameObject _currentItemDescription;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryCanvas.gameObject.SetActive(!inventoryCanvas.gameObject.activeSelf);
            UpdateUI();
        }
        
    }

    public void SortByWeapon()
    {
        _sortByItem = SortByItem.Weapon;
        UpdateUI();
    }
    
    public void SortByArmor()
    {
        _sortByItem = SortByItem.Armor;
        UpdateUI();
    }
    
    public void SortByObtainedDate()
    {
        _sortByItem = SortByItem.Obtained;
        UpdateUI();
    }
    
    public void ShowItemDescription(IRpgObject item, Transform itemTransform)
    {
        DestroyCurrentItemDescription();
        var newPos =  new Vector3(itemTransform.position.x + 300f, itemTransform.position.y - 100f, itemTransform.position.z);
        _currentItemDescription = Instantiate(itemDescriptionPrefab, newPos, Quaternion.identity, inventoryCanvas.transform);
        var texts = _currentItemDescription.GetComponentsInChildren<TextMeshProUGUI>();
        SetUpDescriptionTexts(item, texts);
    }
    
    public void DestroyCurrentItemDescription()
    {
        if (_currentItemDescription != null)
        {
            Destroy(_currentItemDescription);
        }
    }
    
    public void UpdateUI()
    {
        UpdateSlotUI();
        UpdatePlayerStatsUI();
    }

    private void UpdateSlotUI()
    {
        var items = _gameManager.Items;
        var character = _gameManager.Player;
        var slots = inventoryCanvas.GetComponentsInChildren<Image>();
        var equippedItemSlots
            = slots.Where(i => i.gameObject.name.Contains("Equipped")).ToList();
        var existingSlots = inventorySlotCanvas.gameObject.GetComponentsInChildren<Image>().ToList();

        SetUpEquippedItems(equippedItemSlots, character);
        var filteredItems = InventoryUtils.SortItems(items, _sortByItem);
        
        if (_sortByItem == SortByItem.Obtained)
        {
            filteredItems.ForEach(item =>
            {
                existingSlots.ForEach(c => Destroy(c.gameObject));
                InstantiateSlot(item);
            });
            return;
        }
        
        foreach (var item in items)
        {
            if (existingSlots.Count <= 0)
            {
                InstantiateSlot(item);
                continue;
            }
            
            var componentName = "Inventory Slot - " + item.Name;
            var slot = existingSlots.FirstOrDefault(i => i.gameObject.name == componentName);
            if (slot == null)
            {
                InstantiateSlot(item);
                continue;
            }
            
            existingSlots.Remove(slot);

            if (!filteredItems.Contains(item))
            {
                Destroy(slot.gameObject);
            }
        }
    }

    private void InstantiateSlot(IRpgObject item)
    {
        var componentName = "Inventory Slot - " + item.Name;
        var slotComponent = Instantiate(inventorySlotPrefab, inventorySlotCanvas);
        slotComponent.name = componentName;
            
        var slotImage = slotComponent.transform.GetChild(0).GetComponent<Image>();
        slotImage.sprite = item.Sprite;
            
        var button = slotComponent.AddComponent<Button>();
        button.onClick.AddListener(() => item.OnInteract.Invoke());
            
        var controller = slotComponent.GetComponent<InventorySlotController>();
        controller.item = item;
    }

    private void SetUpEquippedItems(List<Image> equippedItemSlots, CharacterImpl character)
    {
        foreach (var slot in equippedItemSlots)
        {
            switch (slot.gameObject.name)
            {
                case "EquippedWeapon":
                    slot.sprite = character.EquippedWeapon.Sprite;
                    break;
                case "EquippedArmor":
                    slot.sprite = character.EquippedArmor.Sprite;
                    break;
            }
        }
    }

    private void SetUpDescriptionTexts(IRpgObject item, TextMeshProUGUI[] textObjects)
    {
        var values = InventoryUtils.GenerateItemDescriptionValues(item);

        for (var i = 0; i < textObjects.Length; i++)
        {
            var textObject = textObjects[i];
            var currentValue = values[i];
            if (currentValue is string[] valueList)
            {
                var split = textObject.text.Split(":");
                var completeText = "";
                for (int j = 0; j < split.Length; j++)
                {
                    var currentReplace = valueList[j];
                    if (j != split.Length - 1) currentReplace += ": ";
                    completeText += split[j].Trim().Replace("replaceable", currentReplace);
                }
                textObject.text = completeText;
            }
            else
            {
                textObject.text = textObject.text.Replace("replaceable", currentValue as string);
            }
            
            textObjects[i] = textObject;
        }
    }

    private void UpdatePlayerStatsUI()
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
                    textComponent.text = text +": " + character.MaxHealth;
                    break;
                case "Base Damage":
                    textComponent.text = text +": " + character.BaseDamage;
                    break;
                case "Base Poise":
                    textComponent.text = text +": " + character.MaxPoise;
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
