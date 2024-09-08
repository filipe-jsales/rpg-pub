using ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterScriptableObject player;
    [SerializeField] 
    private Vector2 deathKnockback = new Vector2(10f,10f);
    [SerializeField] 
    private GameObject bulletPrefab;
    [SerializeField] 
    private Transform firePoint;

    private Vector2 _moveInput;
    private Rigidbody2D _playerRigidBody;
    private SpriteRenderer _spriteRenderer;
    private PlayerController _playerController;
    private Animator _animator;
    private CapsuleCollider2D _playerBodyCollider;
    private BoxCollider2D _playerFeetCollider2D;

    private float _gravityScaleAtStart;
    private bool _isAlive = true;
    private bool _isInvulnerable = false;

    private void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerBodyCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerFeetCollider2D = GetComponent<BoxCollider2D>();
        _gravityScaleAtStart = _playerRigidBody.gravityScale;
        _playerController = GetComponent<PlayerController>();
    }

    public void Update()
    {
        if (!_isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
    }
    void OnMove(InputValue value)
    {
        if (!_isAlive) { return; }
        _moveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2 (_moveInput.x* _playerController.runSpeed, _playerRigidBody.velocity.y);
        _playerRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRigidBody.velocity.x) > Mathf.Epsilon;
        _animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    private void OnJump(InputValue value)
    {
        if (!_isAlive) { return; }
        bool isOnGround = _playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects", "Climbing", "Hazards"));

        if (!isOnGround) return;

        if (value.isPressed)
        {
            _playerRigidBody.velocity += new Vector2(0f, _playerController.jumpSpeed);
        }
    }

    private void OnFire(InputValue value)
    {
        //FIXME: sometimes on enter play it is instatiating a bullet without firing click
        if (!_isAlive) { return; }
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_playerRigidBody.velocity.x), 1f);
        }

    }
    private void ClimbLadder()
    {
        if (!_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            _playerRigidBody.gravityScale = _gravityScaleAtStart;
            _animator.SetBool("isClimbing", false);
            return;
        };
        Vector2 climbVelocity = new Vector2(_playerRigidBody.velocity.x, _moveInput.y * _playerController.climbSpeed);
        _playerRigidBody.velocity = climbVelocity;
        _playerRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(_playerRigidBody.velocity.y) > Mathf.Epsilon;
        _animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            if (_isInvulnerable) return;
            var enemyCharacter = other.gameObject.GetComponent<EnemyController>().EnemyCharacter;
            player.Character.OnHitTaken(enemyCharacter);
            if (player.Character.GetHealth() <= 0)
            {
                _isAlive = false;
                _animator.SetTrigger("isDying");
                _playerRigidBody.velocity = deathKnockback;
                FindAnyObjectByType<GameManager>().ProcessPlayerDeath();
            }
            else
            {
                StartCoroutine(OnDamageTaken());
            }
        }
    }
    private IEnumerator OnDamageTaken()
    {
        Debug.Log("Player is immortal");
        _isInvulnerable = true;
        float elapsed = 0f;
        while (elapsed < _playerController.immortalityDuration)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(_playerController.blinkInterval);
            elapsed += _playerController.blinkInterval;
        }

        _spriteRenderer.enabled = true;
        _isInvulnerable = false;
    }
}
