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
    public event Action <int> OnHealthChange; // returns new health

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
    public void EnemyTurnDecision(bool IsPsychicAffected)
    {
        // if health 100% attack def
        // if health less than 50% then heal half the time, attack and shield split the other half
        // if health greater than 50% attack half the time, heal and shield split

        float percentage = (float) Health / MaxHealth;

        if (percentage == 1)
        {
            // invoke attack
        }
        else if (percentage >= 0.5f)
        {
            // 50 chance of attack, 25 percent chance of defend, 25 percent chance of heal
        }
        else if (percentage <= 0.5f)
        {
            // 50 percent chance of shield, 25 percent of attack, 25 percent of heal
        }
    }

    public void ChangeHealth(int WholeNumberAsPercent)
    {
        int DeltaHealth = (int) GetAsPercentage(WholeNumberAsPercent);

        Health = (Health + DeltaHealth > MaxHealth) ? MaxHealth : (Health + DeltaHealth < 0) ? 0 : Health + DeltaHealth;

        if (Health <= 0)
        {
            Death();
        }

        OnHealthChange?.Invoke(Health);
    }

    private float GetAsPercentage(int wholeNum)
    {
        return (wholeNum / 100f) * MaxHealth;
    }

    private void Death()
    {
        OnDeath?.Invoke();

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
        Destroy(gameObject);
    }
}
