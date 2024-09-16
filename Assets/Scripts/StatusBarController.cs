using Abstractions;
using UnityEngine;
using UnityEngine.Serialization;

public class StatusBarController: MonoBehaviour
{
    public enum Status
    {
        Health, Poise
    }
    [SerializeField]
    private bool isPlayer = false;
    [FormerlySerializedAs("barValue")] [SerializeField]
    private Status barValueType = Status.Health;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        var character = isPlayer ? GameManager.instance.Character : transform.parent.parent.GetComponent<EnemyController>().EnemyCharacter;
        var percentage = 0f;
        switch (barValueType)
        {
            case Status.Health:
                percentage = HandleHealth(character);
                break;
            case Status.Poise:
                percentage = HandlePoise(character);
                break;
        }
        _transform.localScale = new Vector3(percentage <= 0 ? 0 : percentage, 1f);
    }

    private float HandleHealth(Character character)
    {
        return character.Health / character.MaxHealth;
    }
    
    private float HandlePoise(Character character)
    {
        return character.getTotalCurrentPoise() / character.getTotalMaxPoise();
    }
    
}
