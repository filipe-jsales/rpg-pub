using System.Collections;
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

    private void Update()
    {
       StartCoroutine(Run());
    }


    private IEnumerator Run()
    {
        yield return new WaitForSeconds(1);
        UseRunAnimation(gameObject.name);
    }

    public void UseDashAnimation(string objectName) => _character.UseDashAnimation(objectName);
    public void UseRunAnimation(string objectName) => _character.UseRunAnimation(objectName);

}
