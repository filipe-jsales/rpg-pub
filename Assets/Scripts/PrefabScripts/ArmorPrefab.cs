using Abstractions;
using Enums;
using UnityEditor.Animations;
using UnityEngine;

namespace PrefabScripts
{
    public class ArmorPrefab : MonoBehaviour
    {
        [SerializeField]
        private string armorName;
        
        [SerializeField]
        [Header("Weapon Information")]
        private double armorDurability;
    
        [SerializeField]
        private double armorResistance;
    
        [SerializeField]
        private int armorPoise;
        
        public Armor GetArmor()
        {
            return new ArmorImpl(armorName, armorDurability, armorResistance, armorPoise);
        }
    }
}