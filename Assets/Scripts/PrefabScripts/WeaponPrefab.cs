using Abstractions;
using Enums;
using UnityEngine;

namespace PrefabScripts
{
    public class WeaponPrefab : MonoBehaviour
    {
        [SerializeField]
        private WeaponEnum weaponName;

        [SerializeField]
        private RuntimeAnimatorController animatorController;
        
        [SerializeField]
        private Sprite sprite;
        
        [SerializeField]
        [Header("Weapon Information")]
        private double weaponDurability;
    
        [SerializeField]
        private double weaponDamage;
    
        [SerializeField]
        private int weaponPoiseDamage;

        public RuntimeAnimatorController RuntimeAnimatorController => animatorController;
        
        public Weapon GetWeapon()
        {
            var weapon = new WeaponImpl(weaponName.ToString(), weaponDurability, weaponDamage, weaponPoiseDamage);
            weapon.SetSprite(sprite);
            return weapon;
        }
    }
}