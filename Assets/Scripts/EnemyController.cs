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
    
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rigidbody2d;
    [SerializeField] LayerMask solidObjectsLayer;

    public Character EnemyCharacter;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        EnemyCharacter = GenerateEnemyFromParameters();
    }

    void Update()
    {
        rigidbody2d.velocity = new Vector2(moveSpeed, 0f);
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
        transform.localScale = new Vector2(-(Mathf.Sign(rigidbody2d.velocity.x)), 1f);
    }
    
    private CharacterImpl GenerateEnemyFromParameters()
    {
        var weapon = weaponObject.GetComponent<WeaponPrefab>().GetWeapon();
        var armor = armorObject.GetComponent<ArmorPrefab>().GetArmor();
        return new CharacterImpl(characterName, baseHealth, baseDamage, basePoise, armor, weapon);
    }
}
