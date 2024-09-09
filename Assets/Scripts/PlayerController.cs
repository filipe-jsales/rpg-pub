using System.Collections.Generic;
using Interfaces;
using PrefabScripts;
using ScriptableObjects;
using UnityEditor;
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
    private double baseHealth;
    [SerializeField]
    private double baseDamage;
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
            if (player.Character.GetHealth() <= 0)
            {
                player.Character.SetHealth(baseHealth);
            }
            var prefabPath = "Assets/Prefabs/Weapons/";
            equippedWeaponObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + player.Character.Weapon.Name + ".prefab");
        }
        var weaponPrefab = equippedWeaponObject.GetComponent<WeaponPrefab>();
        var animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = weaponPrefab.RuntimeAnimatorController;
        
    }
    
    public void SwitchToWeapon(WeaponPrefab prefab)
    {
        equippedWeaponObject = prefab.gameObject;
        GetComponent<Animator>().runtimeAnimatorController = prefab.AnimatorController;
        player.Character.Weapon = prefab.gameObject.GetComponent<WeaponPrefab>().GetWeapon();
    }
    
    public void SwitchToArmor(ArmorPrefab prefab)
    {
        equippedArmorObject = prefab.gameObject;
        player.Character.Armor = prefab.gameObject.GetComponent<ArmorPrefab>().GetArmor();
    }
    
    private CharacterImpl GeneratePlayerFromParameters()
    {
        var weapon = equippedWeaponObject.GetComponent<WeaponPrefab>().GetWeapon();
        var armor = equippedArmorObject.GetComponent<ArmorPrefab>().GetArmor();
        return new CharacterImpl(characterName, baseHealth, baseDamage, basePoise, armor, weapon);
    }
    
    private IRpgObject[] GetItems()
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
        // TODO: logic for armors

        return items.ToArray();
    }
}
