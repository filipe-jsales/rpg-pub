using Abstractions;
using UnityEngine;
using UnityEngine.Events;

namespace Impl
{
    public class WeaponImpl : Weapon
    {
        public WeaponImpl(string name, Sprite sprite, UnityEvent onInteract, Vector2 knockbackAmount, float durability, float maxDurability, float damage, int poiseDamage)
        {
            Name = name;
            Sprite = sprite;
            OnInteract = onInteract;
            KnockbackAmount = knockbackAmount;
            Durability = durability;
            MaxDurability = maxDurability;
            Damage = damage;
            PoiseDamage = poiseDamage;
        }
    }

}
