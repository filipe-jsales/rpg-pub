using UnityEngine;

public class HealthbarController: MonoBehaviour
{
    [SerializeField]
    private bool isPlayer = false;
    private Transform _healthTransform;

    private void Start()
    {
        _healthTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        var character = isPlayer ? GameManager.instance.Character : transform.parent.parent.GetComponent<EnemyController>().EnemyCharacter;
        var healthPercentage = character.Health / character.MaxHealth;
        _healthTransform.localScale = new Vector3(healthPercentage <= 0 ? 0 : healthPercentage, 1f);
    }
}
