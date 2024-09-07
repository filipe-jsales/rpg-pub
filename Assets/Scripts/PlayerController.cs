using Abstractions;
using PrefabScripts;
using ScriptableObjects;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterScriptableObject player;

    [SerializeField]
    private string characterName;

    [SerializeField]
    [Header("Player Stats")]
    private double baseHealth;
    
    [SerializeField]
    private double baseDamage;
    
    [SerializeField]
    private int basePoise;

    [SerializeField] [Header("Weapon")]
    private GameObject weaponObject;
    
    [SerializeField] [Header("Armor")]
    private GameObject armorObject;

    private string _previousWeaponName;

    private void Start()
    {
        var weaponPrefab = weaponObject.GetComponent<WeaponPrefab>();
        _previousWeaponName = weaponPrefab.GetWeapon().Name;
        var animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = weaponPrefab.AnimatorController;
        player.Character = GeneratePlayerFromParameters();
    }

    private void Update()
    {
        // TODO: verificar impacto de usar get component assim provavelmente um UnityEvent
        var weaponPrefab = weaponObject.GetComponent<WeaponPrefab>();
        var currentWeaponName = weaponPrefab.GetWeapon().Name;
        if (_previousWeaponName != currentWeaponName)
        {
            _previousWeaponName = currentWeaponName;
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = weaponPrefab.AnimatorController;
        }
    }
    
    private CharacterImpl GeneratePlayerFromParameters()
    {
        var weapon = weaponObject.GetComponent<WeaponPrefab>().GetWeapon();
        var armor = armorObject.GetComponent<ArmorPrefab>().GetArmor();
        return new CharacterImpl(characterName, baseHealth, baseDamage, basePoise, armor, weapon);
    }
}
