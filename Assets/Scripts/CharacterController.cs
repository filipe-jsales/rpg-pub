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
    private bool _isDying = false;
    private bool _isInvulnerable = false;
    private bool isOnGround = false;
    private bool _controlsEnabled = true;

    [Header("Player SFX")][SerializeField]
    AudioClip[] footstepSounds;
    [Header("Player Misc")][SerializeField]
    float footstepDelay = 0.03f;
    private float footstepTimer = 0f;
    [SerializeField]
    float climbSoundDelay = 0.3f;
    private float climbSoundTimer = 0f;

    [SerializeField] private float meleeAttackRange = 1.5f;


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
        if (!_isAlive || !_controlsEnabled) { return; }
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
        if (!_isAlive || !_controlsEnabled) { return; }
        _moveInput = value.Get<Vector2>();
    }

    public void DisablePlayerControls()
    {
        _controlsEnabled = false;
        _playerRigidBody.velocity = Vector2.zero;
        _animator.SetBool("isRunning", false);
    }

    public void EnablePlayerControls()
    {
        _controlsEnabled = true;
    }
    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * _playerController.runSpeed, _playerRigidBody.velocity.y);
        _playerRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRigidBody.velocity.x) > Mathf.Epsilon;
        _animator.SetBool("isRunning", playerHasHorizontalSpeed);

        bool isOnGround = CheckIfPlayerIsOnGround();

        if (playerHasHorizontalSpeed && isOnGround || !_controlsEnabled)
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

    void OnMeleeAttack(InputValue value)
    {
        if (!_isAlive || !_controlsEnabled) { return; }
        if (value.isPressed || !_controlsEnabled)
        {
            _animator.SetBool("isAttacking", true);
            StartCoroutine(StopAttacking());
            LayerMask enemyLayer = LayerMask.GetMask("Enemies");
            Vector2 attackDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, meleeAttackRange, enemyLayer);
            Debug.DrawRay(transform.position, attackDirection * meleeAttackRange, Color.red, 0.5f);

            if (hit.collider != null)
            {
                Debug.Log("Inimigo atingido: " + hit.collider.name);
                

                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.OnHitTaken(player.Character);
                }
            }
        }
    }
    
    private IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(0.2f);

        _animator.SetBool("isAttacking", false);
    }

    private void OnJump(InputValue value)
    {
        if (!_isAlive || !_controlsEnabled) { return; }
        bool isOnGround = _playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects", "Climbing", "Hazards"));

        if (!isOnGround || !_controlsEnabled ) return;

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
        if (!_isAlive || !_controlsEnabled) { return; }
        if (value.isPressed)
        {
            _animator.SetBool("isShootingArrow", true);
            StartCoroutine(ShootingArrow());
        }
    }
    private IEnumerator ShootingArrow()
    {
        yield return new WaitForSeconds(0.2f);
        var arrowRotation =
            transform.localScale.x < 0 ? Quaternion.Euler(0, 0, -225) : Quaternion.Euler(0, 0, -45);
        Instantiate(bulletPrefab, firePoint.position, arrowRotation);

        _animator.SetBool("isShootingArrow", false);
    }

    private void OnLandOnTheGround()
    {
        AudioManager.instance.PlayAtPoint("Player Land on the ground");
    }

    private void OnDash(InputValue value)
    {
        if (!_isAlive || !_playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects")) || !_playerController.canDash || !_controlsEnabled) return;

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

        if (playerHasHorizontalSpeed || !_controlsEnabled)
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
        if (!_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) || !_controlsEnabled)
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
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")) || !_controlsEnabled)
        {
            HandleDeath(1);
        }
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) || !_controlsEnabled)
        {
            Vector2 triggerCenter = GetComponent<Collider2D>().bounds.center;
            Vector2 colliderCenter = other.bounds.center;

            Vector2 difference = colliderCenter - triggerCenter;
            var direction = difference.x < 0 ? 1 : -1;
            
            if (_isInvulnerable || !_controlsEnabled) return;
            var enemyCharacter = other.gameObject.GetComponent<EnemyController>().EnemyCharacter;
            player.Character.OnHitTaken(enemyCharacter);
            
            if (player.Character.Health <= 0)
            {
                HandleDeath(direction);
            }
            else
            {
                StartCoroutine(OnDamageTaken(direction));
            }
        }
    }

    private void HandleDeath(int direction)
    {
        if (!_isAlive || !_controlsEnabled) return;
        _isAlive = false;
        _isDying = true;
        _animator.SetTrigger("isDying");
        deathKnockback.x *= direction;
        _playerRigidBody.velocity = deathKnockback;
        FindAnyObjectByType<GameManager>().ProcessPlayerDeath();
        GetComponent<PlayerController>().enabled = false;
    }

    private IEnumerator OnDamageTaken(int direction)
    {
        if (_isDying || !_controlsEnabled) yield break;
        _isInvulnerable = true;
        float elapsed = 0f;
        HandlePoise(direction);
        while (elapsed < _playerController.immortalityDuration)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(_playerController.blinkInterval);
            elapsed += _playerController.blinkInterval;
        }

        _animator.SetBool("isDashing", false);
        _moveInput.x = 0;
        
        _controlsEnabled = true;
        _spriteRenderer.enabled = true;
        _isInvulnerable = false;
    }

    private void HandlePoise(int direction)
    {
        if (player.Character.Poise > 0) return;
        player.Character.HandleBrokenPoise();
        _controlsEnabled = false;
        _animator.SetBool("isDashing", true);
        deathKnockback.x *= direction;
        _playerRigidBody.velocity = deathKnockback;
    }

    private bool CheckIfPlayerIsOnGround()
    {
        return _playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects", "Climbing", "Hazards"));
    }
}
