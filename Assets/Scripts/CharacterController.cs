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
    private bool isOnGround = false;

    [Header("Player SFX")][SerializeField]
    AudioClip[] footstepSounds;
    [Header("Player Misc")][SerializeField]
    float footstepDelay = 0.03f;
    private float footstepTimer = 0f;
    [SerializeField]
    float climbSoundDelay = 0.3f;
    private float climbSoundTimer = 0f;

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
    void FixedUpdate()
    {
        if (_playerRigidBody.velocity.y > _playerController.maxVerticalSpeed)
        {
            _playerRigidBody.velocity = new Vector2(_playerRigidBody.velocity.x,
                Mathf.Lerp(_playerRigidBody.velocity.y, _playerController.maxVerticalSpeed, Time.deltaTime)
                );
        }
    }

    public void Update()
    {
        if (!_isAlive) { return; }
        Run();
        FlipSprite();
        OnClimbLadder();

        bool wasOnGround = isOnGround;
        isOnGround = _playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects"));

        if (!wasOnGround && isOnGround)
        {
            OnLandOnTheGround();
        }

    }
    void OnMove(InputValue value)
    {
        if (!_isAlive) { return; }
        _moveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * _playerController.runSpeed, _playerRigidBody.velocity.y);
        _playerRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRigidBody.velocity.x) > Mathf.Epsilon;
        _animator.SetBool("isRunning", playerHasHorizontalSpeed);

        bool isOnGround = checkIfPlayerIsOnGround();

        if (playerHasHorizontalSpeed && isOnGround)
        {
            GetFootSteps();
        }
    }

    private void GetFootSteps()
    {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            AudioManager.instance.PlayRandomFootstep(transform.position);
            footstepTimer = footstepDelay;
        }
    }

    private void OnJump(InputValue value)
    {
        if (!_isAlive) { return; }
        bool isOnGround = _playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects", "Climbing", "Hazards"));

        if (!isOnGround) return;

        if (value.isPressed)
        {
            AudioManager.instance.PlayAtPoint("Player Jump");
            _playerRigidBody.velocity += new Vector2(0f, _playerController.jumpSpeed);
        }
    }

    private void OnFire(InputValue value)
    {
        //TODO: refactor this method to allow multiple weapons attacks
        //FIXME: sometimes on enter play it is instatiating a bullet without firing click
        if (!_isAlive) { return; }
        if (value.isPressed)
        {
            _animator.SetBool("isShootingArrow", true);
            StartCoroutine(ShootingArrow());
        }
    }
    private IEnumerator ShootingArrow()
    {
        yield return new WaitForSeconds(0.2f);
        Quaternion arrowRotation = Quaternion.Euler(0, 0, -45);
        Instantiate(bulletPrefab, firePoint.position, arrowRotation);

        _animator.SetBool("isShootingArrow", false);
    }

    private void OnLandOnTheGround()
    {
        AudioManager.instance.PlayAtPoint("Player Land on the ground");
    }

    private void OnDash(InputValue value)
    {
        if (!_isAlive || !_playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects")) || !_playerController.canDash) return;

        _animator.SetBool("isDashing", true);

        StartCoroutine(Dash());
    }


    private IEnumerator Dash()
    {
        _playerController.canDash = false;
        float originalSpeed = _playerController.runSpeed;
        _playerController.runSpeed = _playerController.dashSpeed;
        _isInvulnerable = true;

        yield return new WaitForSeconds(_playerController.dashDuration);

        _playerController.runSpeed = originalSpeed;
        _animator.SetBool("isDashing", false);
        _animator.SetBool("isRunning", true);
        _isInvulnerable = false;
        yield return new WaitForSeconds(_playerController.dashCooldown);

        _playerController.canDash = true;
    }

    private void FlipSprite(SpriteRenderer spriteToFlip = null)
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_playerRigidBody.velocity.x), 1f);

            if (spriteToFlip != null)
            {
                spriteToFlip.flipX = transform.localScale.x < 0;
            }
        }
    }
    private void OnClimbLadder()
    {
        if (!_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            _playerRigidBody.gravityScale = _gravityScaleAtStart;
            _animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(_playerRigidBody.velocity.x, _moveInput.y * _playerController.climbSpeed);
        _playerRigidBody.velocity = climbVelocity;
        _playerRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(_playerRigidBody.velocity.y) > Mathf.Epsilon;
        _animator.SetBool("isClimbing", playerHasVerticalSpeed);

        if (playerHasVerticalSpeed)
        {
            climbSoundTimer -= Time.deltaTime;

            if (climbSoundTimer <= 0f)
            {
                AudioManager.instance.PlayAtPoint("Player Climb Ladder");
                climbSoundTimer = climbSoundDelay;
            }
        }
        else
        {
            climbSoundTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            HandleDeath();
        }
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            if (_isInvulnerable) return;
            var enemyCharacter = other.gameObject.GetComponent<EnemyController>().EnemyCharacter;
            player.Character.OnHitTaken(enemyCharacter);
            if (player.Character.GetHealth() <= 0)
            {
                HandleDeath();
            }
            else
            {
                StartCoroutine(OnDamageTaken());
            }
        }
    }

    private void HandleDeath()
    {
        _isAlive = false;
        _animator.SetTrigger("isDying");
        _playerRigidBody.velocity = deathKnockback;
        FindAnyObjectByType<GameManager>().ProcessPlayerDeath();
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
    private bool checkIfPlayerIsOnGround()
    {
        return _playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects", "Climbing", "Hazards"));
    }
}
