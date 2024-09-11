using Abstractions;
using Impl;
using UnityEngine;
using UnityEngine.Events;

namespace PrefabScripts
{
    public class ArmorPrefab : MonoBehaviour
    {
        
        [SerializeField]
        private Sprite sprite;
        
        [SerializeField]
        [Header("Weapon Information")]
        private float armorDurability;
    
        [SerializeField]
        private float armorResistance;
    
        [SerializeField]
        private int armorPoise;
        
        public Armor GetArmor()
        {
            var armor = new ArmorImpl(
                gameObject.name, 
                sprite,
                OnInteract(),
                armorDurability, 
                armorDurability,
                armorResistance, 
                armorPoise,
                armorPoise
            );
            return armor;
        }
        
        private UnityEvent OnInteract()
        {
            var uEvent = new UnityEvent();
            uEvent.AddListener(() =>
            {
                GameObject.Find("Player").GetComponent<PlayerController>().SwitchToArmor(this);
            });

            return uEvent;
        }
    }
}