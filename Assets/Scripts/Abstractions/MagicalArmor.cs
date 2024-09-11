using Interfaces;

namespace Abstractions
{
    public abstract class MagicalArmor : Armor, IMagicalRpgObject
    {
        protected abstract void SetMagicResistance(float value);
        protected abstract void SetResistanceElement(string value);

        public abstract int HandleMagicalDamage(int damage);

        public float MagicFactor
        {
            set => SetMagicResistance(value);
        }

        public string ElementalFactor
        {
            set => SetResistanceElement(value);
        }
    }
}