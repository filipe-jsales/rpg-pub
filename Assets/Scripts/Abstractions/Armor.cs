using System;
using Enums;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Abstractions
{
    public abstract class Armor : IRpgObject, IHasObtainedDate, IDescribable
    {
        public string Name { get; set; }
        public Sprite Sprite { set; get; }
        public DateTime ObtainedDate { get; set; }

        public UnityEvent OnInteract { get; set; }
        
        public float Durability { get; set; }
        public float MaxDurability { get; set; }
        public float PhysicalResistance { get; set; }
        public int Poise { get; set; }
        public int MaxPoise { get; set; }

        public virtual void HandleDurabilityDamage(float damage)
        {
            var durabilityDamage = HandlePhysicalDamage(damage) / 5;
            Durability -= durabilityDamage;
            Debug.Log("Durability after attack: " + Durability);
        }

        public virtual float HandlePhysicalDamage(float damage)
        {
            var conditionBonus = GetArmorConditionBonus();
            var actualDamage = (damage - conditionBonus) * ((100 - PhysicalResistance) / 100);
            return actualDamage < 0 ? 0 : actualDamage;
        }

        public virtual void HandlePoiseDamage(Weapon weapon)
        {
            Poise -= weapon.PoiseDamage;
        }

        public virtual void HandleBrokenPoise()
        {
            Poise = MaxPoise;
        }

        public virtual ArmorCondition GetArmorCondition()
        {
            if (Durability >= 66) return ArmorCondition.Pristine;
            if (Durability >= 33) return ArmorCondition.Damaged;
            return ArmorCondition.Ineffective;
        }
        
        public virtual int GetArmorConditionBonus()
        {
            var condition = GetArmorCondition();
            switch (condition)
            {
                case ArmorCondition.Pristine:
                    return 5;
                case ArmorCondition.Damaged:
                    return 3;
                default:
                    return 0;
            }
        }

        public virtual object[] ToItemDescription()
        {
            return new object[]
            {
                Name, 
                "Armor",
                new [] { "Physical Resistance", PhysicalResistance.ToString()  },
                new [] { "Poise", MaxPoise.ToString()  },
                "Description",
                Durability + "/" + MaxDurability
            };
        }

        void IRpgObject.SetHealthFactor(float value)
        {
            Durability = value;
        }

        void IRpgObject.SetMaxHealthFactor(float value)
        {
            MaxDurability = value;
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
            PhysicalResistance = value;
        }
    }
}
