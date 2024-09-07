using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterScriptableObject player;
    
    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpSpeed = 5.0f;
    [SerializeField] float climbSpeed = 5.0f;
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

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRigidBody.gravityScale;
    }

    public void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        OnPlayerDie();
    }
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x* runSpeed, playerRigidBody.velocity.y);
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
            Debug.Log("jumped");
            playerRigidBody.velocity += new Vector2(0f, jumpSpeed);
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
        Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, moveInput.y * climbSpeed);
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
        }
    }
}
