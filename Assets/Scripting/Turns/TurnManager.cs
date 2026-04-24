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
    [SerializeField] private UnityEvent <TurnState> OnStateChange;

    [SerializeField] private float EnemyTurnTime = 1.2f;
    private float EnemyTurnTimer = 0f;
    private bool EnemyTurnTimerStarted = false;

    [SerializeField] private float PlayerTurnTime = 1.2f;
    private float PlayerTurnTimer = 0f;
    private bool PlayerTurnTimerStarted = false;

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
        EnemyTurnTimer = (EnemyTurnTimer - Time.deltaTime < 0) ? 0 : EnemyTurnTimer - Time.deltaTime;
        if (EnemyTurnTimer <= 0 && EnemyTurnTimerStarted) EndEnemyTurnAfterTimer();

        PlayerTurnTimer = (PlayerTurnTimer - Time.deltaTime < 0) ? 0 : PlayerTurnTimer - Time.deltaTime;
        if (PlayerTurnTimer <= 0 && PlayerTurnTimerStarted) EndPlayerTurnAfterTimer();
    }

    private void Start()
    {
        turnState = TurnState.StartUp;
        OnStartUp?.Invoke();
        OnStateChange?.Invoke(turnState);
    }

    public void EndStartUp()
    {
        //Debug.Log("start up over");
        OnEndStartUp?.Invoke();
        turnState = TurnState.PlayerTurn;
        OnStateChange?.Invoke(turnState);
    }
    public void EndPlayerTurn()
    {
        //Debug.Log("player's turn is over.");
        PlayerTurnTimer = PlayerTurnTime;
        PlayerTurnTimerStarted = true;
    }
    private void EndPlayerTurnAfterTimer()
    {
        PlayerTurnTimerStarted = false;
        turnState = TurnState.EnemyTurn;
        OnPlayerTurnEnd?.Invoke();
        OnStateChange?.Invoke(turnState);
    }

    public void EndEnemyTurn()
    {
        //Debug.Log("enemy turn is ending");
        EnemyTurnTimer = EnemyTurnTime;
        EnemyTurnTimerStarted = true;

    }
    // I could have tied this into animation events... food for thought.
    private void EndEnemyTurnAfterTimer()
    {
        EnemyTurnTimerStarted = false;
        turnState = TurnState.PlayerTurn;
        OnEnemyTurnEnd?.Invoke();
        OnStateChange?.Invoke(turnState);
    }

    public void PlayerDeath()
    {
        //Debug.Log("In turn manager: player dead");
        turnState = TurnState.PlayerDeath;
        OnPlayerDeath?.Invoke();
        OnStateChange?.Invoke(turnState);
    }
    public void LevelComplete()
    {
        //Debug.Log("Level complete");
        turnState = TurnState.LevelCompleted;
        OnLevelComplete?.Invoke();
        OnStateChange?.Invoke(turnState);
    }
}
