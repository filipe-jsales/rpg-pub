using Abstractions;
using Interfaces;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour, IUnityAnimations
{
    private EnemyCharacter _character;

    private void Start()
    {
        _character = GetComponent<EnemyController>().EnemyCharacter;
        AnimationManager.Instance.InitEvent(gameObject.name);
    }

    // private void Update()
    // {
    //    StartCoroutine(Run());
    // }
    //
    //
    // private IEnumerator Run()
    // {
    //     yield return new WaitForSeconds(1);
    //     UseRunAnimation(gameObject.name);
    // }

    public void UseDashAnimation(string objectName, bool isActive) => _character.UseDashAnimation(gameObject.name, isActive);
    public void UseRunAnimation(string objectName, bool isActive) => _character.UseRunAnimation(gameObject.name, isActive);

    public void UseIdleAnimation(string objectName, bool isActive) => _character.UseIdleAnimation(gameObject.name, isActive);

}
