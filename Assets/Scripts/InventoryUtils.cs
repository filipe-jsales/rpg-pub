using System;
using System.Linq;
using Interfaces;

public static class InventoryUtils
{
    public static IRpgObject[] SortByClass(IRpgObject[] items, Type classType)
    {
        if (classType == null)
        {
            throw new ArgumentException("Class name cannot be null or empty.");
        }
        
        var itemsList = items.ToList();

        return itemsList.Where(obj => obj.GetType().IsSubclassOf(classType)).ToArray();
    }

    public static IRpgObject[] SortByObtainedDate(IRpgObject[] items)
    {
        var convertedItems = items.OfType<IHasObtainedDate>().ToList();
        convertedItems.Sort((x, y) => x.ObtainedDate.CompareTo(y.ObtainedDate));
        return convertedItems.OfType<IRpgObject>().ToArray();
    }
}   