using ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterScriptableObject player;
    
    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    [SerializeField] Vector2 deathKnockback = new Vector2(10f,10f);

    Animator animator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider2D;
    public LayerMask solidObjectsLayer;
    public LayerMask interactablesLayer;
    float gravityScaleAtStart;
    bool isAlive = true;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    bool isImmortal = false;
    SpriteRenderer spriteRenderer;

    PlayerController playerController;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRigidBody.gravityScale;
        playerController = GetComponent<PlayerController>();
    }

    public void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("Health: " + player.Character.GetHealth());
        }
    }
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x* playerController.runSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        bool isOnGround = playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects", "Climbing", "Hazards"));

        if (!isOnGround) return;

        if (value.isPressed)
        {
            playerRigidBody.velocity += new Vector2(0f, playerController.jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        //FIXME: sometimes on enter play it is instatiating a bullet without firing click
        if (!isAlive) { return; }
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1f);
        }

    }
    void ClimbLadder()
    {
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerRigidBody.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        };
        Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, moveInput.y * playerController.climbSpeed);
        playerRigidBody.velocity = climbVelocity;
        playerRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
    void OnPlayerDie()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("isDying");
            playerRigidBody.velocity = deathKnockback;
            FindObjectOfType<GameManager>().ProcessPlayerDamageTaken();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isImmortal) return;
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "SolidObjects")))
        {
            var enemyCharacter = other.gameObject.GetComponent<EnemyController>().EnemyCharacter;
            player.Character.OnHitTaken(enemyCharacter);
            if (player.Character.GetHealth() <= 0)
            {
                isAlive = false;
                animator.SetTrigger("isDying");
                playerRigidBody.velocity = deathKnockback;
                FindObjectOfType<GameManager>().ProcessPlayerDamageTaken();
            }
            else
            {
                StartCoroutine(OnDamageTaken());
            }
        };
    }
    IEnumerator OnDamageTaken()
    {
        Debug.Log("Player is immortal");
        isImmortal = true;
        float elapsed = 0f;
        while (elapsed < playerController.immortalityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(playerController.blinkInterval);
            elapsed += playerController.blinkInterval;
        }

        spriteRenderer.enabled = true;
        isImmortal = false;
    }
}
