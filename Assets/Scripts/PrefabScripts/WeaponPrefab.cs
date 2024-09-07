using Abstractions;
using Enums;
using UnityEditor.Animations;
using UnityEngine;

namespace PrefabScripts
{
    public class WeaponPrefab : MonoBehaviour
    {
        [SerializeField]
        private WeaponEnum weaponName;

        [SerializeField]
        private AnimatorController animatorController;
        
        [SerializeField]
        [Header("Weapon Information")]
        private double weaponDurability;
    
        [SerializeField]
        private double weaponDamage;
    
        [SerializeField]
        private int weaponPoiseDamage;

        public AnimatorController AnimatorController => animatorController;
        
        public Weapon GetWeapon()
        {
            return new WeaponImpl(weaponName.ToString(), weaponDurability, weaponDamage, weaponPoiseDamage);
        }
    }
}