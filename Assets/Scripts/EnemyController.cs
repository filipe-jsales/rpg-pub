using System;
using Abstractions;
using Impl;
using PrefabScripts;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private string characterName;

    [SerializeField]
    [Header("Player Stats")]
    private float baseHealth;
    
    [SerializeField]
    private float baseDamage;
    
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
    private CapsuleCollider2D _playerBodyCollider;
    
    public EnemyCharacter EnemyCharacter;

    private void Awake()
    {
        EnemyCharacter = GenerateEnemyFromParameters();
    }

    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerBodyCollider = GetComponent<CapsuleCollider2D>();
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Bullets")))
        {
            if(other.gameObject.name == "Arrow(Clone)") Destroy(other.gameObject);
            AudioManager.instance.PlayAtPoint("Goober Damage");
            var player = GameManager.instance.Player;
            EnemyCharacter.OnHitTaken(player);
            // TODO: call animation controller, knockback = player.EquippedWeapon.Knockback
            if (EnemyCharacter.GetHealth() <= 0)
            {
                Destroy(gameObject);
            }
        };
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_rigidbody2d.velocity.x)), 1f);
    }

    public void OnHitTaken(Character attacker)
    {
        EnemyCharacter.OnHitTaken(attacker);
        if (EnemyCharacter.GetHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    private EnemyCharacterImpl GenerateEnemyFromParameters()
    {
        var weapon = weaponObject.GetComponent<WeaponPrefab>().GetWeapon();
        var armor = armorObject.GetComponent<ArmorPrefab>().GetArmor();
        return new EnemyCharacterImpl(characterName, baseHealth, baseDamage, basePoise, armor, weapon);
    }
}
