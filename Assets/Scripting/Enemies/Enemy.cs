using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private string EnemyName;
    [SerializeField] private GameObject EnemyModel;

    // Percent of health to heal.
    [SerializeField] private Vector2 HealRange;

    // Percent of damage to deal.
    [SerializeField] private Vector2 AttackRange;

    // Percent of damage to take.
    [SerializeField] private Vector2 DefenseRange;

    public string GetEnemyName() => EnemyName;
    public GameObject GetSprite() => EnemyModel;
    public float GetHealthPercentage()
    {
        HealRange = CheckMinMax(HealRange);
        return Random.Range(HealRange.x, HealRange.y);
    }
    public float GetAttackPercentage()
    {
        AttackRange = CheckMinMax(AttackRange);
        return Random.Range(AttackRange.x, AttackRange.y);
    }
    public float GetDefensePercentage() 
    {
        DefenseRange = CheckMinMax(DefenseRange);
        return Random.Range(DefenseRange.x, DefenseRange.y);
    }

    private Vector2 CheckMinMax(Vector2 _range)
    {
        if (_range.y < _range.x)
        {
            float temp = _range.x;
            _range.x = _range.y;
            _range.y = temp;
            Debug.Log("An Enemy has a range (attack, health, or defense) with inverted values (max is less than the min). This was fixed in run time.");
        }
        return _range;
    }
}
