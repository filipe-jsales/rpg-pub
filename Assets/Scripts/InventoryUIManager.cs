using System;
using System.Collections;
using System.Linq;
using Abstractions;
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

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        UpdateInventorySlotImages();
        UpdateTextComponents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryCanvas.gameObject.SetActive(!inventoryCanvas.gameObject.activeSelf);
            UpdateInventorySlotImages();
            UpdateTextComponents();
        }
        
    }
    
    public void SortByWeapon()
    {
        _sortByItem = SortByItem.Weapon;
        UpdateInventorySlotImages();
        UpdateTextComponents();
    }
    
    public void SortByArmor()
    {
        _sortByItem = SortByItem.Armor;
        UpdateInventorySlotImages();
        UpdateTextComponents();
    }
    
    public void SortByObtainedDate()
    {
        _sortByItem = SortByItem.Obtained;
        UpdateInventorySlotImages();
        UpdateTextComponents();
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
        var existingSlots = inventorySlotCanvas.gameObject.GetComponentsInChildren<Image>();

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

        IRpgObject[] filteredItems = null;
        switch (_sortByItem)
        {
            case SortByItem.Armor:
                filteredItems = InventoryUtils.SortByClass(items, typeof(Armor));
                break;
            case SortByItem.Weapon:
                filteredItems = InventoryUtils.SortByClass(items, typeof(Weapon));
                break;
            case SortByItem.Obtained:
                filteredItems = InventoryUtils.SortByObtainedDate(items);
                break;
        }
        
        foreach (var item in items)
        {
            var componentName = "Inventory Slot - " + item.Name;
            // TODO: se for tiver vários do mesmo item, essa lógica vai impedir de aparecer na UI
            if (existingSlots.Length > 0)
            {
                try
                {
                    var image = existingSlots.First(i => i.gameObject.name == componentName);
                    
                    if (image != null)
                    {
                        if (_sortByItem == SortByItem.Obtained)
                        {
                            Destroy(image.gameObject);
                        }
                        else
                        {
                            if (filteredItems != null && filteredItems.Contains(item))
                            {
                                var existingSlotComponent = image.gameObject;
                                existingSlotComponent.name = componentName;
            
                                var existingImage = existingSlotComponent.GetComponent<Image>();
                                existingImage.sprite = item.Sprite;
            
                                var existingController = existingSlotComponent.GetComponent<InventorySlotController>();
                                existingController.item = item;
                            }
                            else
                            {
                                Destroy(image.gameObject);
                            }
                            continue;
                        }
                        
                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            
            
            var slotComponent = Instantiate(inventorySlotPrefab, inventorySlotCanvas);
            slotComponent.name = componentName;
            
            var slotImage = slotComponent.GetComponent<Image>();
            slotImage.sprite = item.Sprite;
            
            var button = slotComponent.AddComponent<Button>();
            button.onClick.AddListener(() => item.OnInteract.Invoke());
            
            var controller = slotComponent.GetComponent<InventorySlotController>();
            controller.item = item;

        }
    }

    private GameObject _currentItemDescription = null;
    public void ShowItemDescription(IRpgObject item, Transform itemTransform)
    {
        DestroyCurrentItemDescription();
        var newPos =  new Vector3(itemTransform.position.x + 300f, itemTransform.position.y - 100f, itemTransform.position.z);
        _currentItemDescription = Instantiate(itemDescriptionPrefab, newPos, Quaternion.identity, inventoryCanvas.transform);
        var texts = _currentItemDescription.GetComponentsInChildren<TextMeshProUGUI>();
        SetUpDescriptionTexts(item, texts);
    }

    private void SetUpDescriptionTexts(IRpgObject item, TextMeshProUGUI[] texts)
    {
        object[] values = GenerateItemDescriptionValues(item);

        for (var i = 0; i < texts.Length; i++)
        {
            var textObject = texts[i];
            var currentValue = values[i];
            if (currentValue is string[])
            {
                var split = textObject.text.Split(":");
                var completeText = "";
                for (int j = 0; j < split.Length; j++)
                {
                    var currentReplace = (currentValue as string[])[j];
                    if (j != split.Length - 1) currentReplace += ": ";
                    completeText += split[j].Trim().Replace("replaceable", currentReplace);
                }
                textObject.text = completeText;
            }
            else
            {
                textObject.text = textObject.text.Replace("replaceable", currentValue as string);
            }
            
            texts[i] = textObject;
        }
    }

    private object[] GenerateItemDescriptionValues(IRpgObject item)
    {
        object[] values;
        if (item.GetType().IsSubclassOf(typeof(Weapon)))
        {
            var itemConverted = item as Weapon;
            values = new object[]
            {
                itemConverted.Name, 
                "Weapon",
                new string[] { "Damage", itemConverted.Damage.ToString()  },
                new string[] { "Poise damage", itemConverted.PoiseDamage.ToString()  },
                "Description",
                itemConverted.Durability + "/" + itemConverted.MaxDurability
            };
        }
        else
        {
            var itemConverted = item as Armor;
            values = new object[]
            {
                itemConverted.Name, 
                "Armor",
                new string[] { "Physical Resistance", itemConverted.PhysicalResistance.ToString()  },
                new string[] { "Poise", itemConverted.MaxPoise.ToString()  },
                "Description",
                itemConverted.Durability + "/" + itemConverted.MaxDurability
            };
        }

        return values;
    }

    public void DestroyCurrentItemDescription()
    {
        if (_currentItemDescription != null)
        {
            Destroy(_currentItemDescription);
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
