using Interfaces;
using UnityEngine;

public class InventorySlotController : MonoBehaviour
{
    public IRpgObject item;

    public void OnCursorEnter()
    {
        GameManager.instance.ShowItemDescription(item, transform);
    }
    public void OnCursorExit()
    {
        GameManager.instance.DestroyCurrentItemDescription();
    }
}