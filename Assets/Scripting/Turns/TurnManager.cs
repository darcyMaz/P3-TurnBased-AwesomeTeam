using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private TurnState turnState;
    [SerializeField] private UnityEvent OnStartUp;
    [SerializeField] private UnityEvent OnEndStartUp;
    [SerializeField] private UnityEvent OnPlayerTurnEnd;
    [SerializeField] private UnityEvent OnEnemyTurnEnd;
    [SerializeField] private UnityEvent OnPlayerDeath;
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
        //Debug.Log(turnState);
    }

    private void Start()
    {
        turnState = TurnState.StartUp;
        OnStartUp?.Invoke();
    }

    public void EndStartUp()
    {
        Debug.Log("start up over");
        OnEndStartUp?.Invoke();
        turnState = TurnState.PlayerTurn;
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
    public void LevelComplete()
    {
        Debug.Log("Level complete");
        turnState = TurnState.LevelCompleted;
        OnLevelComplete?.Invoke();
    }

    public TurnState GetTurnState()
    {
        return turnState;
    }
}
