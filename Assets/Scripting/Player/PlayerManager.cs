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
    [SerializeField] private UnityEvent<int> OnAttack;
    [SerializeField] private UnityEvent<int> OnShield;
    [SerializeField] private UnityEvent<int> OnHeal;

    [SerializeField] private Vector2 AttackRange;
    [SerializeField] private Vector2 ShieldRange;
    [SerializeField] private Vector2 HealRange;

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

        if (MoveName == "Attack") Attack();
        else if (MoveName == "Shield") Shield();
        else if (MoveName == "Heal") Heal();
        else
        {
            Debug.Log("PlayerManager.Move(Movename, IsPsyAttacking) does not have a valid MoveName string.");
        }
    }

    private void Attack()
    {
        OnAttack?.Invoke( GetRange(AttackRange.x, AttackRange.y) );

        if (HasAnim) animator.SetTrigger("IsAttacking");
        else EndPlayerTurn();
    }
    private void Shield()
    {
        OnShield?.Invoke( GetRange(ShieldRange.x, ShieldRange.y) );

        if (HasAnim) animator.SetTrigger("IsShielding");
        else EndPlayerTurn();
    }
    private void Heal()
    {
        OnHeal?.Invoke( GetRange(HealRange.x, HealRange.y) );

        if (HasAnim) animator.SetTrigger("IsHealing");
        else EndPlayerTurn();
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
}
