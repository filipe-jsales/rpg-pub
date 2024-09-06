using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GingerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rigidBody;
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpSpeed = 5.0f;
    [SerializeField] float climbSpeed = 5.0f;

    Animator animator;
    BoxCollider2D boxCollider2D;
    public LayerMask solidObjectsLayer;
    public LayerMask interactablesLayer;
    float gravityScaleAtStart;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidBody.gravityScale;
    }

    public void HandleUpdate()
    {

    }
    public void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();

        if (Input.GetKeyDown(KeyCode.Z))
            Interact();
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x* runSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void OnJump(InputValue value)
    {
        bool isOnGround = boxCollider2D.IsTouchingLayers(LayerMask.GetMask("SolidObjects"));
        bool isOnLadder = boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (!isOnGround && !isOnLadder) return;

        if (value.isPressed)
        {
            rigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x), 1f);
        }

    }
    void Interact()
    {
        var facingDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        var interactPosition = (Vector2)transform.position + facingDirection * 0.5f;

        Debug.DrawLine(transform.position, interactPosition, Color.red, 1f);

        int interactablesLayerMask = LayerMask.GetMask("Interactable"); 

        var collider = Physics2D.OverlapCircle(interactPosition, 0.5f, interactablesLayerMask);

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
        else
        {
            Debug.Log("Não tem um objeto interagível aqui");
        }
    }


    void ClimbLadder()
    {
        if (!boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rigidBody.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        };
        Vector2 climbVelocity = new Vector2(rigidBody.velocity.x, moveInput.y * climbSpeed);
        rigidBody.velocity = climbVelocity;
        rigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
}
