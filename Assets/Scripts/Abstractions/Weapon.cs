using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Abstractions
{
    public abstract class Weapon : IRpgObject
    {
        public string Name { get; set; }
        public Sprite Sprite { get; set; }
        public Vector2 Knockback { get; set; }
        public UnityEvent OnInteract { get; set; }
        protected abstract void SetDurability(float value);
        protected abstract void SetMaxDurability(float value);
        protected abstract void SetDamage(float value);
        protected abstract void SetPoiseDamage(int value);
        
        

        public abstract void HandleDurabilityDamage(Armor armor);
        public abstract float HandlePhysicalDamage(float baseDamage);
        public abstract int HandlePoiseDamage();

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
            set => SetDamage(value);
        }

        public int PoiseFactor
        {
            set => SetPoiseDamage(value);
        }
    }
}