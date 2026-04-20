using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int TotalPsychicAttacks = 3;
    private int CurrentPsychicAttacks = 0;

    [SerializeField] private UnityEvent<int> OnPsychicAttackUse;

    private void Awake()
    {
        CurrentPsychicAttacks = TotalPsychicAttacks;
    }

    public void Attack(string AttackName, bool IsPsyAttacking)
    {
        if (IsPsyAttacking) UsePsyAttack();

        // Perform various attack implemented here
        // I guess they'll each be an event
    }

    private void UsePsyAttack()
    {
        if (CurrentPsychicAttacks <= 0)
        {
            return;
        }
        OnPsychicAttackUse?.Invoke(--CurrentPsychicAttacks);

        // Debug.Log(CurrentPsychicAttacks);
    }
}
