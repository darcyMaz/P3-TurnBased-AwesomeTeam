using UnityEngine;

public enum TurnState
{
    StartUp,
    PlayerTurn, // player chooses a move
    EnemyTurn, // enemy chooses a move
    PlayerMove, // player's move takes effect
    EnemyMove, // enemy move takes effect
    PlayerDeath, // if player dies
    EnemyDeath, // if enemy dies
    LevelCompleted // all enemies dead
}
