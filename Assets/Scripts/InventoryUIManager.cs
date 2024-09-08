using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas inventoryCanvas;

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
        var textComponents = inventoryCanvas.GetComponentsInChildren<TextMeshProUGUI>();
        var images = inventoryCanvas.GetComponentsInChildren<Image>();
        var character = gameObject.GetComponent<GameManager>().Player;
        // TODO: when inventory is implemented, get sprite from item there
        foreach (var i in images)
        {
            // TODO: substitute by a tag
            if (i.gameObject.name == "InventorySlot1")
            {
                i.sprite = character.Weapon.GetSprite();
            }
        }
        // TODO: better this
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
        yield return new WaitForSeconds(2f);
    }
}
