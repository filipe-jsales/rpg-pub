using Abstractions;
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
        private double weaponDurability;
    
        [SerializeField]
        private double weaponDamage;
    
        [SerializeField]
        private int weaponPoiseDamage;

        public RuntimeAnimatorController RuntimeAnimatorController => animatorController;
        
        public Weapon GetWeapon()
        {
            var weapon = new WeaponImpl(gameObject.name, weaponDurability, weaponDamage, weaponPoiseDamage);
            weapon.Sprite = sprite;
            weapon.OnInteract = OnInteract();
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