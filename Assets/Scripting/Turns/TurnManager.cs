using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private TurnState turnState;
    [SerializeField] private UnityEvent OnStartUp;
    [SerializeField] private UnityEvent OnPlayerTurnStart;
    [SerializeField] private UnityEvent OnPlayerTurnEnd;
    [SerializeField] private UnityEvent OnEnemyTurnEnd;
    [SerializeField] private UnityEvent OnPlayerDeath;
    [SerializeField] private UnityEvent OnEnemyDeath;
    [SerializeField] private UnityEvent OnNextEnemy;
    [SerializeField] private UnityEvent OnLevelComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        Debug.Log(turnState);
    }

    private void Start()
    {
        turnState = TurnState.StartUp;
        OnStartUp?.Invoke();
    }

    public void StartPlayerTurn()
    {
        Debug.Log("player's turn has started.");
        turnState = TurnState.PlayerTurn;
        OnPlayerTurnStart?.Invoke();
    }
    public void EndPlayerTurn()
    {
        Debug.Log("player's turn is over.");
        turnState = TurnState.EnemyTurn;
        OnPlayerTurnEnd?.Invoke();
    }
    public void EndEnemyTurn()
    {
        Debug.Log("enemy turn is over");
        turnState = TurnState.PlayerTurn;
        OnEnemyTurnEnd?.Invoke();
    }

    public void PlayerDeath()
    {
        Debug.Log("In turn manager: player dead");
        turnState = TurnState.PlayerDeath;
        OnPlayerDeath?.Invoke();
    }
    public void EnemyDeath()
    {
        Debug.Log("In turn manager: enemy dead");
        turnState = TurnState.EnemyDeath;
        OnEnemyDeath?.Invoke();
    }
    public void NextEnemy()
    {
        Debug.Log("In turn manager: enter next enemy");
        turnState = TurnState.NextEnemyTransition;
        OnNextEnemy?.Invoke();
    }
    public void LevelComplete()
    {
        Debug.Log("Level complete");
        turnState = TurnState.LevelCompleted;
        OnLevelComplete?.Invoke();
    }


}
