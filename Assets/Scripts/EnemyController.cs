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
    private EnemyAnimationController _animationController;
    private bool _isPlayerDetected = false;

    [SerializeField]
    private bool startMovingRight = true;
    [SerializeField]
    private float detectionRange = 3f;
    [SerializeField]
    private LayerMask playerLayer;

    private void Awake()
    {
        if (startMovingRight) moveSpeed = -moveSpeed;
        EnemyCharacter = GenerateEnemyFromParameters();
    }

    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerBodyCollider = GetComponent<CapsuleCollider2D>();
        _animationController = GetComponent<EnemyAnimationController>();

    }
    
    void FixedUpdate()
    {
        _animationController.UseRunAnimation("", false);
        var hit = Physics2D.Raycast(transform.position, -transform.right, 0.1f, solidObjectsLayer);
        DetectPlayer();

        if (hit.collider != null)
        {
            FlipSprite();
        }

        if (_isPlayerDetected)
        {
            ChasePlayer(_playerBodyCollider.transform);
        }
        else
        {
            Idle();
        }
    }

    private void FlipSprite()
    {
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector2(-transform.localScale.x, 1f);
    }

    private void ChasePlayer(Transform playerTransform)
    {
        //TODO refactor this
        if (gameObject.name == "Bat (Enemy)")
        {
            BatChasingPlayer(playerTransform);
        }
        if (gameObject.name == "Goober (Enemy)")
        {
            GooberChasingPlayer(playerTransform);
        }
    }

    private void GooberChasingPlayer(Transform playerTransform)
    {
        float direction = transform.position.x - playerTransform.position.x;

        if (direction > 0 && transform.localScale.x < 0)
        {
            FlipSprite();
        }
        else if (direction < 0 && transform.localScale.x > 0)
        {
            FlipSprite();
        }

        _rigidbody2d.velocity = new Vector2(Mathf.Sign(direction) * moveSpeed, _rigidbody2d.velocity.y);
        _animationController.UseRunAnimation("", true);
    }

    private void BatChasingPlayer(Transform playerTransform)
    {
        float directionX = playerTransform.position.x - transform.position.x;
        float directionY = playerTransform.position.y - transform.position.y;

        if (directionX > 0 && transform.localScale.x < 0)
        {
            FlipSprite();
        }
        else if (directionX < 0 && transform.localScale.x > 0)
        {
            FlipSprite();
        }

        _rigidbody2d.velocity = new Vector2(Mathf.Sign(directionX) * moveSpeed, Mathf.Sign(directionY) * moveSpeed);

        _animationController.UseRunAnimation("", true);

        Debug.Log($"Bat chasing player. Horizontal: {directionX}, Vertical: {directionY}");
    }


    private void Idle()
    {
        //TODO refactor this
        if (gameObject.name == "Bat (Enemy)")
        {
            BatIdling();
        }
        if (gameObject.name == "Goober (Enemy)")
        {
            GooberIdling();
        }

    }

    private void GooberIdling()
    {
        _rigidbody2d.velocity = new Vector2(0f, _rigidbody2d.velocity.y);
        _animationController.UseRunAnimation("", false);
    }

    private void BatIdling()
    {
        DetectCeiling();
    }

    private void DetectCeiling()
    {
        // Lança um Raycast para cima
        RaycastHit2D ceilingHit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, solidObjectsLayer);

        Debug.DrawLine(transform.position, transform.position + new Vector3(0, 1, 0), Color.red);

        // Se o teto foi encontrado
        if (ceilingHit.collider != null)
        {
            // Calcula a distância vertical até o teto
            float distanceToCeiling = ceilingHit.point.y - transform.position.y;

            // Se estamos próximos o suficiente do teto, o morcego para de subir e fica ocioso
            if (distanceToCeiling < 0.2f)
            {
                _rigidbody2d.velocity = Vector2.zero; // Para o movimento
                _animationController.UseRunAnimation("", false); // Volta à animação ociosa
                Debug.Log("Bat is idling on the ceiling");
            }
            else
            {
                // Se ainda não alcançou o teto, move o morcego para cima
                _rigidbody2d.velocity = new Vector2(0f, Mathf.Abs(moveSpeed)); // Certifica que moveSpeed é positivo
                _animationController.UseRunAnimation("", true); // Continua com a animação de voar
                Debug.Log("Bat is moving towards the ceiling");
            }
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
            HandleKnockback(player);
            HandleDeath();
        };
    }

    private void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        Debug.DrawLine(transform.position, transform.position + new Vector3(detectionRange, 0, 0), Color.red);

        if (playerCollider != null && playerCollider.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            _isPlayerDetected = true;
            ChasePlayer(playerCollider.transform);
        }
        else
        {
            _isPlayerDetected = false;
            Idle();
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
