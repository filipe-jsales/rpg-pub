using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    private float bulletSpeed = 10f;

    Rigidbody2D bulletRigidBody;
    CharacterController characterController;
    float xSpeed;
    void Start()
    {
        bulletRigidBody = GetComponent<Rigidbody2D>();
        characterController = FindFirstObjectByType<CharacterController>();
        xSpeed = characterController.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        bulletRigidBody.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (bulletRigidBody.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            AudioManager.instance.PlayAtPoint("Goober Damage");
            var enemyCharacter = other.gameObject.GetComponent<EnemyController>().EnemyCharacter;
            enemyCharacter.OnHitTaken(GameManager.instance.Player);
            if (enemyCharacter.GetHealth() <= 0)
            {
                Destroy(other.gameObject);
            }
        };
        Destroy(gameObject);
    }

}
