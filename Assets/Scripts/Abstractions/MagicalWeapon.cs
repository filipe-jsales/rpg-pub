using Interfaces;

namespace Abstractions
{
    public abstract class MagicalWeapon : Weapon, IMagicalRpgObject
    {
        protected abstract void SetMagicDamage(double value);
        protected abstract void SetElementalAffinity(string value);

        public abstract int HandleMagicalDamage(int damage);

        public double MagicFactor
        {
            set => SetMagicDamage(value);
        }

        public string ElementalFactor
        {
            set => SetElementalAffinity(value);
        }
    }
}