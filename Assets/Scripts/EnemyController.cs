using System;
using System.Collections;
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

    [SerializeField]
    private bool startMovingRight = true;

    private void Awake()
    {
        if(startMovingRight) moveSpeed = -moveSpeed;
        EnemyCharacter = GenerateEnemyFromParameters();
    }

    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerBodyCollider = GetComponent<CapsuleCollider2D>();
        
    }
    
    void FixedUpdate()
    {
        // Check for solid objects in front
        var hit = Physics2D.Raycast(transform.position, -transform.right, 0.1f, solidObjectsLayer);

        if (hit.collider != null)
        {
            // Reverse direction if a solid object is found
            moveSpeed = -moveSpeed;
            transform.localScale = new Vector2(-transform.localScale.x, 1f);
        }
        
        // Move the character
        _rigidbody2d.velocity = new Vector2(moveSpeed, _rigidbody2d.velocity.y);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Bullets")))
        {
            if(other.gameObject.name == "Arrow(Clone)") Destroy(other.gameObject);
            AudioManager.instance.PlayAtPoint("Goober Damage");
            var player = GameManager.instance.Player;
            
            EnemyCharacter.OnHitTaken(player);
            HandleKnockback(player);
            HandleDeath();
        };
    }

    public void OnHitTaken(Character attacker)
    {
        EnemyCharacter.OnHitTaken(attacker);
        var player = GameManager.instance.Player;
        
        HandleKnockback(player);
        HandleDeath();
    }

    private void HandleKnockback(Character player)
    {
        var poise = EnemyCharacter.getTotalCurrentPoise();
        if (poise <= 0)
        {
            EnemyCharacter.HandleBrokenPoise();
            Vector2 knockbackAmount = player.EquippedWeapon.KnockbackAmount;
            // TODO: do knockback
        }
    }
    
    private void HandleDeath()
    {
        if (EnemyCharacter.Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    private EnemyCharacterImpl GenerateEnemyFromParameters()
    {
        var weapon = weaponObject.GetComponent<WeaponPrefab>().GetWeapon();
        var armor = armorObject.GetComponent<ArmorPrefab>().GetArmor();
        return new EnemyCharacterImpl(
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
}
