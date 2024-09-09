using Abstractions;
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
        private double armorDurability;
    
        [SerializeField]
        private double armorResistance;
    
        [SerializeField]
        private int armorPoise;
        
        public Armor GetArmor()
        {
            var armor = new ArmorImpl(gameObject.name, armorDurability, armorResistance, armorPoise);
            armor.Sprite = sprite;
            armor.OnInteract = OnInteract();
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