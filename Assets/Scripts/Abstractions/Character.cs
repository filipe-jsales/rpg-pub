using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Abstractions
{
    public abstract class Character : IRpgObject, IMagicalRpgObject, ILeveling, IEquipment, IHitDetectable
    {
        public string Name { get; set; }
        public Sprite Sprite { get; set; }
        public UnityEvent OnInteract { get; set; }
        public int Level { get; set; }
        public double Experience { get; set; }
        
        public float BaseDamage { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public int Poise { get; set; }
        public int MaxPoise { get; set; }
        public float Mana { get; set; }
        public float MaxMana { get; set; }
        public string ElementAffinity { get; set; }
        
        public Armor EquippedArmor { get; set; }
        public Weapon EquippedWeapon { get; set; }
        
        public virtual float OnHit(Armor armor)
        {
            EquippedWeapon.HandleDurabilityDamage(armor);
            return EquippedWeapon.HandlePhysicalDamage(BaseDamage);
        }

        public virtual void OnHitTaken(Character attacker)
        {
            var damage = attacker.OnHit(EquippedArmor);
            var damageTaken = EquippedArmor.HandlePhysicalDamage(damage);
            EquippedArmor.HandleDurabilityDamage(damage);
            EquippedArmor.HandlePoiseDamage(attacker.EquippedWeapon);
            HandlePoiseDamage(attacker.EquippedWeapon);
            Health -= damageTaken;
        }

        public virtual void HandlePoiseDamage(Weapon weapon)
        {
            Poise -= weapon.PoiseDamage;
            EquippedArmor.HandlePoiseDamage(weapon);
        }
        
        public virtual void HandleBrokenPoise()
        {
            Poise = MaxPoise;
            EquippedArmor.HandleBrokenPoise();
        }
        
        public virtual void HandleDeath()
        {
            Health = MaxHealth;
        }

        public virtual int getTotalCurrentPoise()
        {
            return Poise + EquippedArmor.Poise;
        }

        void IRpgObject.SetHealthFactor(float value)
        {
            Health = value;
        }

        void IRpgObject.SetMaxHealthFactor(float value)
        {
            MaxHealth = value;
        }

        void IRpgObject.SetPoiseFactor(int value)
        {
            Poise = value;
        }

        void IRpgObject.SetMaxPoiseFactor(int value)
        {
            MaxPoise = value;
        }

        void IRpgObject.SetDamageFactor(float value)
        {
            BaseDamage = value;
        }

        void IMagicalRpgObject.SetMagicFactor(float value)
        {
            Mana = value;
        }

        void IMagicalRpgObject.SetMaxMagicFactor(float value)
        {
            MaxMana = value;
        }

        void IMagicalRpgObject.SetElementalFactor(string value)
        {
            ElementAffinity = value;
        }

        void IEquipment.SetArmor(Armor armor)
        {
            EquippedArmor = armor;
        }

        void IEquipment.SetWeapon(Weapon armor)
        {
            EquippedWeapon = armor;
        }
    }
}