using System;
using System.Collections.Generic;
using Impl;
using Interfaces;
using PrefabScripts;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterScriptableObject player;
    [SerializeField]
    private string characterName;
    
    [Header("Player Stats")]
    [SerializeField]
    private float baseHealth;
    [SerializeField]
    private float baseDamage;
    [SerializeField]
    private int basePoise;
    [SerializeField] 
    public float runSpeed = 5.0f;
    [SerializeField] 
    public float jumpSpeed = 5.0f;
    [SerializeField] 
    public float climbSpeed = 5.0f;
    
    [FormerlySerializedAs("weaponObject")]
    [Header("Weapon")]
    [SerializeField] 
    private GameObject equippedWeaponObject;
    
    [FormerlySerializedAs("weapons")] [SerializeField] 
    private GameObject[] startingWeapons;
    
    [Header("Armor")]
    [SerializeField] 
    private GameObject equippedArmorObject;
    
    [SerializeField] 
    private GameObject[] startingArmors;

    [Header("Player Misc")]
    [SerializeField] public float maxVerticalSpeed = 10f;
    [SerializeField] public float immortalityDuration = 2f;
    [SerializeField] public float blinkInterval = 0.1f;
    [SerializeField] public float dashSpeed = 5f;
    [SerializeField] public float dashDuration = 0.3f;
    [SerializeField] public float dashCooldown = 2f;
    public bool canDash = true;
    

    private void Start()
    {
        if (player.Character == null)
        {
            player.Character = GeneratePlayerFromParameters();
            player.Items = GetItems();
        }
        else
        {
            if (player.Character.Health <= 0) player.Character.HandleDeath();
            
            var prefabPath = "Prefabs/Weapons/";
            equippedWeaponObject = Resources.Load<GameObject>(prefabPath + player.Character.EquippedWeapon.Name);
        }
        var weaponPrefab = equippedWeaponObject.GetComponent<WeaponPrefab>();
        var animator = GetComponent<Animator>();
        // animator.runtimeAnimatorController = weaponPrefab.RuntimeAnimatorController;
        
    }
    
    // TODO: change both below methods to GameManager probably
    public void SwitchToWeapon(WeaponPrefab prefab)
    {
        equippedWeaponObject = prefab.gameObject;
        GetComponent<Animator>().runtimeAnimatorController = prefab.RuntimeAnimatorController;
        player.Character.EquippedWeapon = prefab.gameObject.GetComponent<WeaponPrefab>().GetWeapon();
        GameManager.instance.UpdateInventoryUI();
    }
    
    public void SwitchToArmor(ArmorPrefab prefab)
    {
        equippedArmorObject = prefab.gameObject;
        player.Character.EquippedArmor = prefab.gameObject.GetComponent<ArmorPrefab>().GetArmor();
        GameManager.instance.UpdateInventoryUI();
    }
    
    private CharacterImpl GeneratePlayerFromParameters()
    {
        var weapon = equippedWeaponObject.GetComponent<WeaponPrefab>().GetWeapon();
        var armor = equippedArmorObject.GetComponent<ArmorPrefab>().GetArmor();
        return new CharacterImpl(
            characterName, 
            null,
            null,
            1,
            0,
            baseDamage, 
            baseHealth,
            baseHealth,
            basePoise, 
            basePoise,
            0,
            0,
            "",
            armor, 
            weapon
        );
    }
    
    private List<IRpgObject> GetItems()
    {
        var items = new List<IRpgObject>();

        foreach (var weapon in startingWeapons)
        {
            items.Add(weapon.GetComponent<WeaponPrefab>().GetWeapon());
        }
        
        foreach (var armor in startingArmors)
        {
            items.Add(armor.GetComponent<ArmorPrefab>().GetArmor());
        }

        return items;
    }
}
