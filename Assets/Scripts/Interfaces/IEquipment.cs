using Abstractions;

namespace Interfaces
{
    public interface IEquipment
    {
        //Different name per implementation attributes
        protected void SetArmor(Armor armor);
        protected void SetWeapon(Weapon armor);
    }
}