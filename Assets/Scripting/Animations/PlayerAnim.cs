using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    // Move animations from PM to here and put these back in.
    //private Animator animator;
    //private bool HasAnim = false;

    [SerializeField] GameObject heal;
    private MoveAnim healMA;
    private bool HasHealMA = false;

    [SerializeField] GameObject shield;
    private MoveAnim shieldMA;
    private bool HasShieldMA = false;

    [SerializeField] GameObject attack;
    private MoveAnim attackMA;
    private bool HasAttackMA = false;


    private void Awake()
    {
        if (!heal.TryGetComponent(out healMA)) Debug.Log("An EnemyAnim could not find its MoveAnim for the heal object.");
        else HasHealMA = true;

        if (!shield.TryGetComponent(out shieldMA)) Debug.Log("An EnemyAnim could not find its MoveAnim for the shield object.");
        else HasShieldMA = true;

        if (!attack.TryGetComponent(out attackMA)) Debug.Log("An EnemyAnim could not find its MoveAnim for the attack object.");
        else HasAttackMA = true;

        //if (!TryGetComponent(out animator)) Debug.Log("An EnemyAnim component could not find its own Animator.");
        //else HasAnim = true;
    }

    public void AnimateHeal()
    {
        if (HasHealMA) healMA.TurnOnMoveAnim();
    }
    public void AnimateShield()
    {
        if (HasShieldMA) shieldMA.TurnOnMoveAnim();
    }
    public void AnimateAttack()
    {
        if (HasAttackMA) attackMA.TurnOnMoveAnim();
    }
}
