using Abstractions;
using PrefabScripts;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
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
    
    [SerializeField] 
    private float moveSpeed = 1f;
    [SerializeField] 
    private LayerMask solidObjectsLayer;
    
    private Rigidbody2D _rigidbody2d;

    public Character EnemyCharacter;
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        EnemyCharacter = GenerateEnemyFromParameters();
    }

    void Update()
    {
        _rigidbody2d.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & solidObjectsLayer) != 0)
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_rigidbody2d.velocity.x)), 1f);
    }
    
    private CharacterImpl GenerateEnemyFromParameters()
    {
        var weapon = weaponObject.GetComponent<WeaponPrefab>().GetWeapon();
        var armor = armorObject.GetComponent<ArmorPrefab>().GetArmor();
        return new CharacterImpl(characterName, baseHealth, baseDamage, basePoise, armor, weapon);
    }
}
