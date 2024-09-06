using Abstractions;

namespace Interfaces
{
    public interface IEquipment
    {
        Armor Armor { set; }
        Weapon Weapon { set; }
    }
}