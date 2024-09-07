using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRigidBody;
    [SerializeField] float bulletSpeed = 10f;
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
        if (bulletRigidBody.IsTouchingLayers(LayerMask.GetMask("Enemies", "SolidObjects")))
        {
            Debug.Log("Is touching enemies layer");
        };
        Destroy(gameObject);
    }

}
