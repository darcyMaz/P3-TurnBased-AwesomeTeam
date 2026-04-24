using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    // on player turn end, battle manager takes enemy effects and gives it to player
    // on enemy turn end, battle manager takes effect info and gives it to player

    private int PlayerDefenseToApply = 0;
    private int EnemyDefenseToApply = 0;

    // Events send the exact amount of health to reduce or heal.
    [SerializeField] private UnityEvent <int, bool> OnAttackEnemy;
    [SerializeField] private UnityEvent <int> OnAttackPlayer;
    [SerializeField] private UnityEvent <int> OnHealEnemy;
    [SerializeField] private UnityEvent <int> OnHealPlayer;

    [SerializeField] private UnityEvent OnPlayerTurnEnd;
    [SerializeField] private UnityEvent OnEnemyTurnEnd;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Functions receive PERCENTAGE of health to reduce, defend, or heal.
    public void ReceivePlayerMove(int damage, int defense, int health, bool IsPsychic)
    {
        Debug.Log("Player Move Received " + damage + " : BM.ReceivePlayerMove()");

        PlayerDefenseToApply = defense;

        if (EnemyDefenseToApply > 0) Debug.Log("The Player's Attack was reduced by a previous Defense move by the Enemy.");
        damage = (damage + PlayerDefenseToApply > 0) ? 0 : damage + EnemyDefenseToApply;
        EnemyDefenseToApply = 0;

        if (health > 0) OnHealPlayer?.Invoke(health);

        // If we are not doing damage to the enemy, then there will be no check for enemy death in the Enemy Manager. So, invoke the next turn here.
        if (damage < 0)
        {
            Debug.Log("OnAttackEnemy: BM.ReceivePlayerMove()");
            OnAttackEnemy?.Invoke(damage, IsPsychic);
        }
        else
        {
            OnPlayerTurnEnd?.Invoke();
        }
    }


    public void ReceiveEnemyMove(int damage, int defense, int health)
    {
        Debug.Log("Enemy Move Received in BM " + damage);

        EnemyDefenseToApply = defense;

        if (PlayerDefenseToApply > 0) Debug.Log("The Enemy's Attack was reduced by a previous Defense move by the Player."); // debug statement
        damage = (damage + PlayerDefenseToApply > 0) ? 0 : damage + PlayerDefenseToApply;
        PlayerDefenseToApply = 0;

        // Make an event that says, defense applied

        if (health > 0) OnHealEnemy?.Invoke(health);

        if (damage < 0) OnAttackPlayer?.Invoke(damage);
        else OnEnemyTurnEnd?.Invoke();
    }

}
