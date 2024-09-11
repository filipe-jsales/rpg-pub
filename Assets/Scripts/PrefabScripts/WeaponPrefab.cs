using System;
using Abstractions;
using Impl;
using UnityEngine;
using UnityEngine.Events;

namespace PrefabScripts
{
    public class WeaponPrefab : MonoBehaviour
    {
        [SerializeField]
        private RuntimeAnimatorController animatorController;
        
        [SerializeField]
        private Sprite sprite;
        
        [SerializeField]
        [Header("Weapon Information")]
        private float weaponDurability;
    
        [SerializeField]
        private float weaponDamage;
    
        [SerializeField]
        private int weaponPoiseDamage;
        [SerializeField]
        private float weaponKnockbackValue;

        public RuntimeAnimatorController RuntimeAnimatorController => animatorController;
        
        public Weapon GetWeapon()
        {
            var weapon = new WeaponImpl(
                gameObject.name,
                sprite,
                OnInteract(),
                new Vector2(weaponKnockbackValue, weaponKnockbackValue),
                weaponDurability, 
                weaponDurability, 
                weaponDamage, 
                weaponPoiseDamage
            );
            return weapon;
        }

        private UnityEvent OnInteract()
        {
            var uEvent = new UnityEvent();
            uEvent.AddListener(() =>
            {
                GameObject.Find("Player").GetComponent<PlayerController>().SwitchToWeapon(this);
            });

            return uEvent;
        }
    }
}