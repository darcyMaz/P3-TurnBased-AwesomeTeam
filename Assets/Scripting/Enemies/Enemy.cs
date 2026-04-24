using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{

    private EnemyAnim animator;
    private bool HasAnim = false;

    [SerializeField] private EnemySO data;
    private bool HasData = false;

    private int MaxHealth;
    private int Health;

    [SerializeField] private Vector3 DebugStartPos;

    [SerializeField] private UnityEvent OnAttack;
    [SerializeField] private UnityEvent OnDefend;
    [SerializeField] private UnityEvent OnHeal;

    private string LastDecision = "";
    // 


    // These are C# classes because Enemy is created at runtime, and cannot be assigned in the inspector.
    // UnityEvents can be assigned in this way I bet
    public event Action OnDeath;

    private void Awake()
    {
        if (!TryGetComponent(out animator)) Debug.Log("An Enemy tried to get its animator but couldn't find it.");
        else HasAnim = true;

        if (data == null) Debug.Log("An Enemy tried to get its EnemySO component but could not find it.");
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
            animator.AnimateSlide();
        }
        else
        {
            Debug.Log("The slide animation was not triggered because the Enemy did not have its EnemyAnim component.");
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
        float chance = UnityEngine.Random.Range(0f, 1f);

        float PsychicMultiplier = (IsPsychicAffected) ? data.GetPsychicEffect() : 1;

        if (percentage == 1)
        {
            int Decision = (int)(-data.GetAttackPercentage() * PsychicMultiplier);
            LastDecision = GetName() + " attacked! " + Decision + "%";

            // If at full health, always attack.
            returnVals[0] = Decision;
            OnAttack?.Invoke();
            
        }
        else if (percentage >= 0.5f)
        {
            // 50 chance of attack, 25 percent chance of defend, 25 percent chance of heal
            
            if (chance <= 0.5f)
            {
                int Decision = (int)(-data.GetAttackPercentage() * PsychicMultiplier);
                LastDecision = GetName() + " attacked! " + Decision + "%";

                // Attack
                returnVals[0] = Decision;
                OnAttack?.Invoke();
            }
            else if (chance > 0.5f && chance < 0.75f)
            {
                int Decision = (int)(data.GetDefensePercentage() * PsychicMultiplier);
                LastDecision = GetName() + " shieled themselves for next turn! " + Decision + "%";

                // shield
                returnVals[1] = Decision;
                OnDefend?.Invoke();
            }
            else
            {
                int Decision = (int)(data.GetHealPercentage() * PsychicMultiplier);
                LastDecision = GetName() + " healed! " + Decision + "%";

                // heal
                returnVals[2] = Decision;
                OnHeal?.Invoke();
            }
        }
        else if (percentage < 0.5f)
        {
            // 50 percent chance of shield, 25 percent of attack, 25 percent of heal
            if (chance <= 0.5f)
            {
                int Decision = (int)(data.GetDefensePercentage() * PsychicMultiplier);
                LastDecision = GetName() + " shieled themselves for next turn! " + Decision + "%";

                // Shield
                returnVals[1] = Decision;
                OnAttack?.Invoke();
            }
            else if (chance > 0.5f && chance < 0.75f)
            {
                int Decision = (int)(-data.GetAttackPercentage() * PsychicMultiplier);
                LastDecision = GetName() + " attacked! " + Decision + "%";

                // Attack
                returnVals[0] = Decision;
                OnDefend?.Invoke();
            }
            else
            {
                int Decision = (int)(data.GetHealPercentage() * PsychicMultiplier);
                LastDecision = GetName() + " healed! " + Decision + "%";

                // heal
                returnVals[2] = Decision;
                OnHeal?.Invoke();
            }
        }

        return returnVals;
    }

    public string GetLastDecision()
    {
        return LastDecision;
    }

    public int ChangeHealth(int WholeNumberAsPercent)
    {
        int DeltaHealth = (int) GetAsPercentage(WholeNumberAsPercent);

        Health = (Health + DeltaHealth > MaxHealth) ? MaxHealth : (Health + DeltaHealth < 0) ? 0 : Health + DeltaHealth;

        //Debug.Log("Received Player Attack " + Health + " " + DeltaHealth + ": Enemy.ChangeHealth");

        if (Health <= 0)
        {
            Death();
        }

        return Health;
    }

    public string GetName()
    {
        return data.GetEnemyName();
    }
    public int GetCurrentHealth()
    {
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
            animator.AnimateDeath();
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
