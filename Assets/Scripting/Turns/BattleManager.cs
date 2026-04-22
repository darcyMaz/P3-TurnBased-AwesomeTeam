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
    [SerializeField] private UnityEvent <int> OnAttackEnemy;
    [SerializeField] private UnityEvent <int> OnAttackPlayer;
    [SerializeField] private UnityEvent <int> OnHealEnemy;
    [SerializeField] private UnityEvent <int> OnHealPlayer;

    // 
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
        PlayerDefenseToApply = defense;

        if (EnemyDefenseToApply > 0) Debug.Log("The Player's Attack was reduced by a previous Defense move by the Enemy.");
        damage -= EnemyDefenseToApply;

        if (damage > 0) OnAttackEnemy?.Invoke(damage);
        if (health > 0) OnHealPlayer?.Invoke(health);

        EnemyDefenseToApply = 0;

        OnPlayerTurnEnd?.Invoke();
    }


    public void ReceiveEnemyMove(int damage, int defense, int health)
    {
        EnemyDefenseToApply = defense;

        if (PlayerDefenseToApply > 0) Debug.Log("The Enemy's Attack was reduced by a previous Defense move by the Player.");
        damage -= PlayerDefenseToApply;

        if (damage > 0) OnAttackPlayer?.Invoke(damage);
        if (health > 0) OnHealEnemy?.Invoke(health);

        PlayerDefenseToApply = 0;

        OnEnemyTurnEnd?.Invoke();
    }

}
