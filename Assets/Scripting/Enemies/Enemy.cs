using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Animator animator;
    private bool HasAnim = false;

    private EnemySO data;
    private bool HasData = false;

    private int MaxHealth;
    private int Health;

    [SerializeField] private Vector3 DebugStartPos;

    // These are C# classes because Enemy is created at runtime, and cannot be assigned in the inspector.
    // UnityEvents can be assigned in this way I bet
    public event Action OnDeath;

    private void Awake()
    {
        if (!TryGetComponent(out animator)) Debug.Log("An Enemy tried to get its animator but couldn't find it.");
        else HasAnim = true;

        if (!TryGetComponent(out data)) Debug.Log("An Enemy tried to get its EnemySO component but could not find it.");
        else HasData = true;

        if (HasData)
        {
            Health = data.GetMaxHealth();
            MaxHealth = data.GetMaxHealth();
        }
    }

    // Enemy walks up to starting point.
    public void QueueIn()
    {
        if (HasAnim)
        {
            animator.SetTrigger("Walk In");
        }
        else
        {
            transform.position = DebugStartPos;
        }
    }

    // All Enemys will have the same decision making, albeit with different values.
    // Psychic affect can double or halve whatever the enemy does. Probably halves it.
    public int[] EnemyTurnDecision(bool IsPsychicAffected)
    {
        // if health 100% attack def
        // if health less than 50% then heal half the time, attack and shield split the other half
        // if health greater than 50% attack half the time, heal and shield split

        int[] returnVals = new int[3];

        float percentage = (float) Health / MaxHealth;
        float chance = UnityEngine.Random.Range(0, 1);

        float PsychicMultiplier = (IsPsychicAffected) ? data.GetPsychicEffect() : 1;

        if (percentage == 1)
        {
            // If at full health, always attack.
            returnVals[0] = (int) (data.GetAttackPercentage() * PsychicMultiplier);
        }
        else if (percentage >= 0.5f)
        {
            // 50 chance of attack, 25 percent chance of defend, 25 percent chance of heal
            
            if (chance <= 0.5f)
            {
                // Attack
                returnVals[0] = (int) (data.GetAttackPercentage() * PsychicMultiplier);
            }
            else if (chance > 0.5f && chance < 0.75f)
            {
                // shield
                returnVals[1] = (int) (data.GetDefensePercentage() * PsychicMultiplier);
            }
            else
            {
                // heal
                returnVals[2] = (int) (data.GetHealPercentage() * PsychicMultiplier);
            }
        }
        else if (percentage <= 0.5f)
        {
            // 50 percent chance of shield, 25 percent of attack, 25 percent of heal
            if (chance <= 0.5f)
            {
                // Attack
                returnVals[1] = (int) (data.GetDefensePercentage() * PsychicMultiplier);
            }
            else if (chance > 0.5f && chance < 0.75f)
            {
                // shield
                returnVals[0] = (int) (data.GetAttackPercentage() * PsychicMultiplier);
            }
            else
            {
                // heal
                returnVals[2] = (int) (data.GetHealPercentage() * PsychicMultiplier);
            }
        }

        return returnVals;
    }

    public int ChangeHealth(int WholeNumberAsPercent)
    {
        int DeltaHealth = (int) GetAsPercentage(WholeNumberAsPercent);

        Health = (Health + DeltaHealth > MaxHealth) ? MaxHealth : (Health + DeltaHealth < 0) ? 0 : Health + DeltaHealth;

        if (Health <= 0)
        {
            Death();
        }

        return Health;
    }

    private float GetAsPercentage(int wholeNum)
    {
        return (wholeNum / 100f) * MaxHealth;
    }

    private void Death()
    {
        if (HasAnim)
        {
            animator.SetBool("IsDying", true);
            // This animation will have the destroy game object called in an event
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
