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
    private Enemy CurrentEnemyClone;

    [SerializeField] private UnityEvent OnEnemiesDefeated;
    [SerializeField] private UnityEvent OnNextEnemy;

    [SerializeField] private UnityEvent <int, int, int> OnEnemyMove;
    [SerializeField] private UnityEvent <int> OnHealthChange;
    [SerializeField] private UnityEvent <string> OnNameChange;
    [SerializeField] private UnityEvent OnEnemySurvivedAttack;

    private bool IsNextMovePsychicEffected = false;

    public void EnemyTurn()
    {
        int[] enemyMoveVals = CurrentEnemyClone.EnemyTurnDecision(IsNextMovePsychicEffected);

        Debug.Log("EnemyTurn! "+ enemyMoveVals[0] +" "+ enemyMoveVals[1] +" "+ enemyMoveVals[2] + " : EM.EnemyTurn()");

        OnEnemyMove?.Invoke(enemyMoveVals[0], enemyMoveVals[1], enemyMoveVals[2]);
    }

    // what about when we change health from a heal move? does that mean it'll always say false. ugh
    public void ChangeCurrentEnemyHealth(int WholeNumPercentage, bool IsPsychic)
    {
        Debug.Log("Change Enemy Health " + WholeNumPercentage + " isPsychic " + IsPsychic + ": EM.ChangeCurrentEnemyHealth()");

        IsNextMovePsychicEffected = IsPsychic;
        int NewHealth = CurrentEnemyClone.ChangeHealth(WholeNumPercentage);
        OnHealthChange?.Invoke(NewHealth);

        // if wholenumper < 0 and we're still alive
        //  then enemysurvivedattack invoke 

        if (WholeNumPercentage < 0 && NewHealth > 0)
        {
            Debug.Log("The Enemy survived an attack from the player " + WholeNumPercentage + " " + NewHealth+ " : EM.ChangeCurrentEnemyHealth()");
            OnEnemySurvivedAttack?.Invoke();
        }

        // i realize now that we don't need an enemy death event because we get the val here
    }

    // Call this when the previous enemy died also on start.
    public void TryQueueInEnemy()
    {
        if (EnemyIndex == enemies.Count - 1)
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
            OnNextEnemy?.Invoke();
        }
    }

    public void QueueInEnemy()
    {
        // Spawn in the next enemy where this EnemyManager is the parent transform.
        GameObject CurrentEnemyCloneGO = Instantiate(enemies[EnemyIndex++], transform);

        if (!CurrentEnemyCloneGO.TryGetComponent(out CurrentEnemyClone)) Debug.Log("An enemy prefab did not have its Enemy component. Failed to queue in.");
        else
        {
            CurrentEnemyClone.QueueIn();
            CurrentEnemyClone.OnDeath += EnemyDeath;

            OnNameChange?.Invoke(CurrentEnemyClone.GetName());
            OnHealthChange?.Invoke(CurrentEnemyClone.GetCurrentHealth());
        }
    }

    private void EnemyDeath()
    {
        Debug.Log("Enemy death event successfully listened to.");

        // Unsubscribe the Enemy from this function.
        CurrentEnemyClone.OnDeath -= EnemyDeath;
        // Now we can destroy it.
        Destroy(CurrentEnemyClone.gameObject);

        // Try to queue in the next enemy, turn manager will take the reigns from there.
        TryQueueInEnemy();
    }
}
