using Abstractions;
using Interfaces;
using UnityEngine;

public class EnemyActionController : MonoBehaviour, IEnemyActionController
{
    private EnemyCharacter _character;

    private void Start()
    {
        _character = GetComponent<EnemyController>().EnemyCharacter;
        EnemyActionManager.Instance.InitEvent(gameObject.name);
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

    public void UseDashAnimation(bool isActive) => _character.UseDashAnimation(gameObject.name, isActive);
    public void UseRunAnimation(bool isActive) => _character.UseRunAnimation(gameObject.name, isActive);

    public void UseIdleAnimation(bool isActive) => _character.UseIdleAnimation(gameObject.name, isActive);

}
