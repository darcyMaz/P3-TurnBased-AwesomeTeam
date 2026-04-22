using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    // many enemies in a list, rather their prefabs!
    // active enemy obj
    // is enemy dead bool, start true
    // if (enemy is dead), pop the next enemy off the queue and spawn it in
    // on enemy turn start, calculate the move to do, some randomness some predictability as he describes
    // launch that move

    // enemy script:
    // start: activate the slide in animation
    // public func listening to battle managers on enemy heal, on enemy attack, on enemy defend... maybe these should be buffered in

    [SerializeField] private List<GameObject> enemies;
    private int EnemyIndex = 0;
    private GameObject CurrentEnemyClone;

    [SerializeField] private UnityEvent OnEnemiesDefeated;

    [SerializeField] private UnityEvent <int, int, int> OnEnemyMove;

    public void EnemyTurn()
    {
        // CurrentEnemyClone.EnemyTurnDecision(bool isPsyActive)
    }

    // Call this when the previous enemy died also on start.
    public void TryQueueInEnemy()
    {
        if (EnemyIndex == enemies.Count -1)
        {
            // No more enemies
            OnEnemiesDefeated?.Invoke();
        }
        else if (EnemyIndex > enemies.Count - 1 || EnemyIndex < 0)
        {
            // improper index
            Debug.Log("EnemyManager tried to queue up the next enemy but the index was out of range.");
        }
        else
        {
            // Next enemy please!
            QueueInEnemy();
        }
    }

    private void QueueInEnemy()
    {
        CurrentEnemyClone = Instantiate(enemies[EnemyIndex++]);
        Enemy en;

        if (TryGetComponent(out en)) en.QueueIn();
        else Debug.Log("An enemy prefab did not have its Enemy component. Failed to queue in.");
    
    }
}
