using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int TotalPsychicAttacks = 3;
    private int CurrentPsychicAttacks = 0;

    private Animator animator;
    private bool HasAnim = false;

    [SerializeField] private UnityEvent<int> OnPsychicAttackUse;
    [SerializeField] private UnityEvent<int, int, int, bool> OnMove;

    // Represents the range of move effect as whole number percentages.
    [SerializeField] private Vector2 AttackRange;
    [SerializeField] private Vector2 ShieldRange;
    [SerializeField] private Vector2 HealRange;

    // Returns changed health to listeners.
    [SerializeField] private UnityEvent <int> OnHealthChange;
    [SerializeField] private int MaxHealth;
    private int Health;

    [SerializeField] private UnityEvent OnTurnEnd;
    [SerializeField] private UnityEvent OnDeath;

    private void Awake()
    {
        CurrentPsychicAttacks = TotalPsychicAttacks;
        Health = MaxHealth;

        if (Health <= 0)
        {
            Debug.Log("The Player has started with a Health less than or equal to zero. Restarting the game.");
            Death();
        }

        if (!TryGetComponent(out animator)) Debug.Log("PlayerManager could not find Animator.");
        else HasAnim = true;
    }

    

    public void Move(string MoveName, bool IsPsyAttacking)
    {
        if (IsPsyAttacking) UsePsyAttack();

        if (MoveName == "Attack") Attack(IsPsyAttacking);
        else if (MoveName == "Shield") Shield(IsPsyAttacking);
        else if (MoveName == "Heal") Heal(IsPsyAttacking);
        else
        {
            Debug.Log("PlayerManager.Move(Movename, IsPsyAttacking) does not have a valid MoveName string.");
        }
    }

    private void Attack(bool IsPsy)
    {
        OnMove?.Invoke( GetRange(AttackRange.x, AttackRange.y), 0, 0, IsPsy );

        if (HasAnim) animator.SetTrigger("IsAttacking");
        else EndPlayerTurn();
    }
    private void Shield(bool IsPsy)
    {
        OnMove?.Invoke( 0, GetRange(ShieldRange.x, ShieldRange.y), 0, IsPsy );

        if (HasAnim) animator.SetTrigger("IsShielding");
        else EndPlayerTurn();
    }
    private void Heal(bool IsPsy)
    {
        OnMove?.Invoke( 0, 0, GetRange(HealRange.x, HealRange.y), IsPsy );

        // Idea here is to have an event in the animations that Ends the turn
        if (HasAnim) animator.SetTrigger("IsHealing");
        else EndPlayerTurn();
    }

    public void ChangeHealth(int WholeNumberAsPercent)
    {
        int DeltaHealth = (int) GetAsPercentage(WholeNumberAsPercent);

        Health = (Health + DeltaHealth > MaxHealth) ? MaxHealth: (Health + DeltaHealth < 0) ? 0: Health + DeltaHealth;

        if (Health <= 0)
        {
            // player dies
            Death();
        }

        OnHealthChange?.Invoke(Health);
    }

    private void EndPlayerTurn()
    {
        OnTurnEnd?.Invoke();
    }

    private void UsePsyAttack()
    {
        if (CurrentPsychicAttacks <= 0)
        {
            return;
        }
        OnPsychicAttackUse?.Invoke(--CurrentPsychicAttacks);
    }


    private void Death()
    {
        OnDeath?.Invoke();
    }

    private int GetRange(float x, float y)
    {
        // I wonder if every game will have the same rand seed

        System.Random rand = new System.Random();
        return rand.Next((int) x, (int) y);
    }

    private float GetAsPercentage(int wholeNum)
    {
        // System.Random rand = new System.Random();
        // float wholeNum = (float)rand.Next((int)x, (int)y);
        return (wholeNum / 100f) * MaxHealth;
    }
}
