using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Interfaces;

public static class InventoryUtils
{
    public static List<IRpgObject> SortByClass(List<IRpgObject> items, Type classType)
    {
        if (classType == null)
        {
            throw new ArgumentException("Class name cannot be null or empty.");
        }
        
        var itemsList = items.ToList();

        return itemsList.Where(obj => obj.GetType().IsSubclassOf(classType)).ToList();
    }

    public static List<IRpgObject> SortByObtainedDate(List<IRpgObject> items)
    {
        var convertedItems = items.OfType<IHasObtainedDate>().ToList();
        convertedItems.Sort((x, y) => x.ObtainedDate.CompareTo(y.ObtainedDate));
        return convertedItems.OfType<IRpgObject>().ToList();
    }
    
    public static List<IRpgObject> SortItems(List<IRpgObject> items, SortByItem sortBy)
    {
        switch (sortBy)
        {
            case SortByItem.Armor:
                return SortByClass(items, typeof(Armor));
            case SortByItem.Weapon:
                return SortByClass(items, typeof(Weapon));
            case SortByItem.Obtained:
                return SortByObtainedDate(items);
        }

        return items;
    }

    public static object[] GenerateItemDescriptionValues(IRpgObject item)
    {
        if (item is IDescribable describableItem)
        {
            return describableItem.ToItemDescription();
        }
        throw new ArgumentException("Item must implement IDescribable.");
    }
}   