using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Abstractions
{
    public abstract class Character : IRpgObject, IMagicalRpgObject, ILeveling, IEquipment, IHitDetectable
    {
        public abstract void SetHealth(double value);
        protected abstract void SetBaseDamage(double value);
        protected abstract void SetBasePoise(int value);
        protected abstract void SetMana(double value);
        protected abstract void SetElementalAffinity(string value);
        protected abstract void SetLevel(int value);
        protected abstract void SetExperience(double value);
        protected abstract Armor GetEquippedArmor();
        protected abstract void SetEquippedArmor(Armor value);
        protected abstract Weapon GetEquippedWeapon();
        protected abstract void SetEquippedWeapon(Weapon value);

        public string Name { get; set; }
        public Sprite Sprite { get; set; }
        public UnityEvent OnInteract { get; set; }

        public abstract double GetHealth();

        public abstract double OnHit(Armor armor);
        public abstract void OnHitTaken(Character attacker);

        public int Level
        {
            set => SetLevel(value);
        }

        public double Experience
        {
            set => SetExperience(value);
        }

        public Armor Armor
        {
            get => GetEquippedArmor();
            set => SetEquippedArmor(value);
        }

        public Weapon Weapon
        {
            get => GetEquippedWeapon();
            set => SetEquippedWeapon(value);
        }

        public double HealthFactor
        {
            set => SetHealth(value);
        }

        public double DamageFactor
        {
            set => SetBaseDamage(value);
        }

        public int PoiseFactor
        {
            set => SetBasePoise(value);
        }

        public double MagicFactor
        {
            set => SetMana(value);
        }

        public string ElementalFactor
        {
            set => SetElementalAffinity(value);
        }
    }
}