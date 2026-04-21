using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private TurnState turnState;

    public void EndPlayerTurn(string MoveType, bool IsPsyAttackUsed)
    {
        Debug.Log("player's turn is over.");
        turnState = TurnState.EnemyTurn;

    }
    public void EndEnemyTurn()
    {
        Debug.Log("enemy turn is over");
        turnState = TurnState.PlayerTurn;
    }

    
}
