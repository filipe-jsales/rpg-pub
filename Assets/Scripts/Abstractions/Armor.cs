
using Interfaces;
using UnityEngine;

namespace Abstractions
{
    public abstract class Armor : IRpgObject
    {
        protected abstract void SetDurability(double value);
        protected abstract void SetPhysicalResistance(double value);
        protected abstract void SetPoise(int value);
        public abstract void HandleDurabilityDamage(double damage);
        public abstract double HandlePhysicalDamage(double damage);
        public abstract void HandlePoiseDamage(Weapon weapon);

        public string Name { get; set; }
        public Sprite Sprite { get; set; }

        public double HealthFactor
        {
            set => SetDurability(value);
        }

        public double DamageFactor
        {
            set => SetPhysicalResistance(value);
        }

        public int PoiseFactor
        {
            set => SetPoise(value);
        }
    }
}
