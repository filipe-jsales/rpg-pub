
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Abstractions
{
    public abstract class Armor : IRpgObject
    {
        protected abstract void SetDurability(float value);
        protected abstract void SetMaxDurability(float value);
        protected abstract void SetPhysicalResistance(float value);
        protected abstract void SetPoise(int value);
        public abstract void HandleDurabilityDamage(float damage);
        public abstract float HandlePhysicalDamage(float damage);
        public abstract void HandlePoiseDamage(Weapon weapon);

        public string Name { get; set; }
        public Sprite Sprite { get; set; }
        public UnityEvent OnInteract { get; set; }

        public float HealthFactor
        {
            set => SetDurability(value);
        }
        
        public float MaxHealthFactor
        {
            set => SetMaxDurability(value);
        }

        public float DamageFactor
        {
            set => SetPhysicalResistance(value);
        }

        public int PoiseFactor
        {
            set => SetPoise(value);
        }
    }
}
