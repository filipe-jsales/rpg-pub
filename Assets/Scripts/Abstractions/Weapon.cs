using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Abstractions
{
    public abstract class Weapon : IRpgObject, IHasObtainedDate, IDescribable
    {
        public string Name { get; set; }
        public Sprite Sprite { get; set; }
        public DateTime ObtainedDate { get; set; }

        public UnityEvent OnInteract { get; set; }
        public Vector2 KnockbackAmount { get; set; }

        public float Durability { get; set; }
        public float MaxDurability { get; set; }
        public float Damage { get; set; }
        public int PoiseDamage { get; set; }

        public virtual void HandleDurabilityDamage(Armor armor)
        {
            var attemptedDamage = armor.HandlePhysicalDamage(Damage);
            Durability -= Damage / attemptedDamage;
        }

        public virtual float HandlePhysicalDamage(float baseDamage)
        {
            return Damage + baseDamage;
        }
        
        public virtual object[] ToItemDescription()
        {
            return new object[]
            {
                Name, 
                "Weapon",
                new [] { "Damage", Damage.ToString()  },
                new [] { "Poise damage", PoiseDamage.ToString()  },
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
            PoiseDamage = value;
        }

        void IRpgObject.SetMaxPoiseFactor(int value)
        {
            throw new System.NotImplementedException();
        }

        void IRpgObject.SetDamageFactor(float value)
        {
            Damage = value;
        }
    }
}