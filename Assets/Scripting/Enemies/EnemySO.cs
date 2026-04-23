using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private string EnemyName;
    [SerializeField] private GameObject EnemyModel; // maybe not this? idk

    // Percent of health to heal.
    [SerializeField] private Vector2 HealRange;

    // Percent of damage to deal.
    [SerializeField] private Vector2 AttackRange;

    // Percent of damage to take.
    [SerializeField] private Vector2 DefenseRange;

    // A Psychic Attack can Double an Enemy's Move, Halve it, or do nothing,
    // This range represents the odds of Doubling and Halving, whereas the difference between 1 and the sum of these two represents the do nothing chance.
    [SerializeField] private Vector2 PsychicEffectRange;

    [SerializeField] private int MaxHealth;

    public string GetEnemyName() => EnemyName;
    public GameObject GetSprite() => EnemyModel;
    public int GetHealPercentage()
    {
        HealRange = CheckMinMax(HealRange);
        return GetRandInt((int) HealRange.x, (int) HealRange.y);
    }
    public int GetAttackPercentage()
    {
        AttackRange = CheckMinMax(AttackRange);
        return GetRandInt((int) AttackRange.x, (int) AttackRange.y);
    }
    public int GetDefensePercentage() 
    {
        DefenseRange = CheckMinMax(DefenseRange);
        return GetRandInt((int)DefenseRange.x, (int)DefenseRange.y);
    }

    public int GetMaxHealth()
    {
        return MaxHealth;
    }

    public float GetPsychicEffect()
    {
        float chance = UnityEngine.Random.Range(0, 1);

        if (chance >= 0 && chance < PsychicEffectRange.x) return 2;//double
        else if (chance >= PsychicEffectRange.x && chance < PsychicEffectRange.y) return 0.5f; // halve
        else if (chance >= PsychicEffectRange.y && chance <= 1) return 1; // do nothing
        else
        {
            Debug.Log("An EnemySO tried to calculate the Psychic Effect but got a value out of range. This should not be possible.");
            return 1; // Outside the range do nothing and mention it.
        }
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

    private int GetRandInt(int min, int max)
    {
        System.Random rand = new System.Random();
        return rand.Next(min, max + 1);
    }
}
