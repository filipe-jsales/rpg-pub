using Abstractions;
using UnityEngine;
using UnityEngine.Events;

namespace Impl
{
    public class CharacterImpl: Character
    {
        public CharacterImpl(string name, Sprite sprite, UnityEvent onInteract, int level, double experience, float baseDamage, float health, float maxHealth, int poise, int maxPoise, float mana, float maxMana, string elementAffinity, Armor equippedArmor, Weapon equippedWeapon)
        {
            Name = name;
            Sprite = sprite;
            OnInteract = onInteract;
            Level = level;
            Experience = experience;
            BaseDamage = baseDamage;
            Health = health;
            MaxHealth = maxHealth;
            Poise = poise;
            MaxPoise = maxPoise;
            Mana = mana;
            MaxMana = maxMana;
            ElementAffinity = elementAffinity;
            EquippedArmor = equippedArmor;
            EquippedWeapon = equippedWeapon;
        }
    }

}