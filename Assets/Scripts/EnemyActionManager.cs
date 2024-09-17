using System.Collections.Generic;
using UnityEngine;

public delegate void UseAnimatorDelegate(Animator animator);
public delegate void UseRigidBodyDelegate(Rigidbody2D rigidbody2D);
public delegate void UseTransformDelegate(Transform transform);
public delegate void UseCapsuleColliderDelegate(CapsuleCollider2D collider);
public delegate void UseAllDelegate(Transform transform, Animator animator, Rigidbody2D rigidbody2D, CapsuleCollider2D collider);
public class EnemyActionManager: MonoBehaviour
{
    public static EnemyActionManager Instance; 
    
    private Dictionary<string, EnemyUnityClasses> _actions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InitEvent(string objectName) => Initialize(objectName);

    public void UseAnimator(string objectName, UseAnimatorDelegate callback) => callback(Instance._actions[objectName].Animator);
    
    public void UseTransform(string objectName, UseTransformDelegate callback) => callback(Instance._actions[objectName].Transform);
    
    public void UseRigidBody(string objectName, UseRigidBodyDelegate callback) => callback(Instance._actions[objectName].Rigidbody);
    
    public void UseCapsuleCollider(string objectName, UseCapsuleColliderDelegate callback) => callback(Instance._actions[objectName].CapsuleCollider2D);
    public void UseAll(string objectName, UseAllDelegate callback) => callback(Instance._actions[objectName].Transform, Instance._actions[objectName].Animator, Instance._actions[objectName].Rigidbody, Instance._actions[objectName].CapsuleCollider2D);

    private void Initialize(string objectName) {
        var enemyGameObject = GameObject.Find(objectName);
        var unityClasses = new EnemyUnityClasses();
        
        var enemyTransform = enemyGameObject.GetComponent<Transform>();
        var enemyRigidbody2D = enemyGameObject.GetComponent<Rigidbody2D>();
        var capsuleCollider2D = enemyGameObject.GetComponent<CapsuleCollider2D>();
        var animator = enemyGameObject.GetComponent<Animator>();
        
        unityClasses.Transform ??= enemyTransform;
        unityClasses.Rigidbody ??= enemyRigidbody2D;
        unityClasses.CapsuleCollider2D ??= capsuleCollider2D;
        unityClasses.Animator ??= animator;
        
        if (_actions == null)
        {
            Instance._actions = new Dictionary<string, EnemyUnityClasses> { { objectName, unityClasses } };
            return;
        }
        if (_actions.ContainsKey(objectName)) 
            Instance._actions[objectName] = unityClasses;
        else 
            Instance._actions.Add(objectName, unityClasses);
        
    }

    public void Reset()
    {
        _actions = null;
    }
}

