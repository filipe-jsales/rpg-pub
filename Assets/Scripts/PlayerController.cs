using System.Collections.Generic;
using System.Linq;
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
    
    [SerializeField] 
    private GameObject[] weapons;
    
    [Header("Armor")]
    [SerializeField] 
    private GameObject armorObject;

    [Header("Player Misc")]
    [SerializeField] public float maxVerticalSpeed = 10f;
    [SerializeField] public float immortalityDuration = 2f;
    [SerializeField] public float blinkInterval = 0.1f;
    [SerializeField] public float dashSpeed = 5f;
    [SerializeField] public float dashDuration = 0.3f;
    [SerializeField] public float dashCooldown = 2f;
    public bool canDash = true;

    private string _previousWeaponName;

    public void SwitchToAnotherRandomWeapon()
    {
        var weaponPrefab = equippedWeaponObject.GetComponent<WeaponPrefab>();
        var prefabs = weapons.Select(o => o.GetComponent<WeaponPrefab>()).ToList();
        prefabs.Remove(weaponPrefab);
        var randomPrefab = prefabs[Random.Range(0, prefabs.Count)];
        equippedWeaponObject = randomPrefab.gameObject;
        Debug.Log("Changed to: " + equippedWeaponObject.gameObject.GetComponent<WeaponPrefab>().GetWeapon().Name);
    }

    private void Start()
    {
        equippedWeaponObject = GetComponent<PlayerController>().equippedWeaponObject;
        if (player.Character == null)
        {
            var weaponPrefab = equippedWeaponObject.GetComponent<WeaponPrefab>();
            _previousWeaponName = weaponPrefab.GetWeapon().Name;
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = weaponPrefab.AnimatorController;
            player.Character = GeneratePlayerFromParameters();
            player.Items = GetItems();
        }
        
    }

    private IRpgObject[] GetItems()
    {
        var items = new List<IRpgObject>();

        foreach (var weapon in weapons)
        {
            items.Add(weapon.GetComponent<WeaponPrefab>().GetWeapon());
        }
        // TODO: logic for armors

        return items.ToArray();
    }

    private void Update()
    {
        // TODO: verificar impacto de usar get component assim provavelmente um UnityEvent
        var weaponPrefab = equippedWeaponObject.GetComponent<WeaponPrefab>();
        var currentWeaponName = player.Character.Weapon.Name;
        if (_previousWeaponName == null) _previousWeaponName = currentWeaponName;
        if ((_previousWeaponName != currentWeaponName) || _previousWeaponName != weaponPrefab.GetWeapon().Name)
        {
            _previousWeaponName = currentWeaponName;
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = weaponPrefab.AnimatorController;
            // TODO: remove this to update the equipped weapon, probably create setWeapon in interface
            player.Character = GeneratePlayerFromParameters();
        }
    }
    
    private CharacterImpl GeneratePlayerFromParameters()
    {
        var weapon = equippedWeaponObject.GetComponent<WeaponPrefab>().GetWeapon();
        var armor = armorObject.GetComponent<ArmorPrefab>().GetArmor();
        return new CharacterImpl(characterName, baseHealth, baseDamage, basePoise, armor, weapon);
    }
}
