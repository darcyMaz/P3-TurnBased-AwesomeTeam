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

    // [SerializeField] private UnityEvent OnTurnEnd;
    [SerializeField] private UnityEvent OnDeath;
    [SerializeField] private UnityEvent OnPlayerSurvivedAttack;

    [SerializeField] private UnityEvent OnAttack;
    [SerializeField] private UnityEvent OnDefend;
    [SerializeField] private UnityEvent OnHeal;


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

    private void Start()
    {
        OnHealthChange?.Invoke(Health);
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
        int temp = -GetRange(AttackRange.x, AttackRange.y);
        //Debug.Log("Player Attack " + temp + " :PM.Attack()");

        OnMove?.Invoke( temp, 0, 0, IsPsy );

        OnAttack?.Invoke();
    }
    private void Shield(bool IsPsy)
    {
        OnMove?.Invoke( 0, GetRange(ShieldRange.x, ShieldRange.y), 0, IsPsy );

        OnDefend?.Invoke();
    }
    private void Heal(bool IsPsy)
    {
        int temp = GetRange(HealRange.x, HealRange.y);
        //Debug.Log("Player Heals " + temp + ": PM.Heal()");

        OnMove?.Invoke( 0, 0, temp, IsPsy );

        OnHeal?.Invoke();
    }

    public void ChangeHealth(int WholeNumberAsPercent)
    {
        int DeltaHealth = (int) GetAsPercentage(WholeNumberAsPercent);

        // If the player is being attacked and the percentage rounds down to zero, round it up to 1.
        if (WholeNumberAsPercent < 0 && DeltaHealth == 0) DeltaHealth = -1;

        Health = (Health + DeltaHealth > MaxHealth) ? MaxHealth: (Health + DeltaHealth < 0) ? 0: Health + DeltaHealth;

        //Debug.Log("Health " + Health + "DeltaHealth " +DeltaHealth+ ": PM.ChangeHealth()");


        if (Health <= 0)
        {
            //Debug.Log("Health <= 0: PM.ChangeHealth()");

            // player dies
            Death();
        }
        else
        {
            // communicate that it's the enemy's turn now... wait
            // i'll have to know which turn we're in because change health is for both turns

            // if player is alive and if WholeNumberAsPercent < 0: then we are being attacked and are still alive
            // it is the enemy's turn, they attacked us, and we are still alive
            // next, it should be the player's turn

            if (DeltaHealth < 0) OnPlayerSurvivedAttack?.Invoke();
        }

        OnHealthChange?.Invoke(Health);
    }

    private void UsePsyAttack()
    {
        if (CurrentPsychicAttacks <= 0)
        {
            return;
        }
        OnPsychicAttackUse?.Invoke(--CurrentPsychicAttacks);
    }

    public void VictoryAnim()
    {
        if (HasAnim)
        {
            animator.SetTrigger("VictoryTrigger");
        }
    }

    private void Death()
    {
        //Debug.Log("PM.Death()");
        if (HasAnim)
        {
            // Animation will call DestroyPlayer()
            animator.SetTrigger("DeathTrigger");
        }
        else
        {
            DestroyPlayer();
        }
    }

    public void DestroyPlayer()
    {
        //Debug.Log("PM.DestroyPlayer()");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    private int GetRange(float x, float y)
    {
        // I wonder if every game will have the same rand seed
        System.Random rand = new System.Random();
        return rand.Next((int) x, (int) y);
    }

    private float GetAsPercentage(int wholeNum)
    {
        return (wholeNum / 100f) * MaxHealth;
    }
}
