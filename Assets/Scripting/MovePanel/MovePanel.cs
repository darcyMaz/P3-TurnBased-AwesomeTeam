using System;
using UnityEngine;
using UnityEngine.Events;

public class MovePanel : MonoBehaviour
{
    // all buttons start as not selected.
    // if you press one it is marked as selected and deselects the others
    // exception is psychic

    // So, Attack Heal and Defend share one variable.
    // Psychic has a simple on off variable.

    // On Go, if psychic is selected apply the buff or nerf or nothing
    // Which ever other one is selected do that move

    private string MoveSelected = "";
    private bool IsPsychicSelected = false;

    [SerializeField] private UnityEvent <string> OnButtonSelected;
    [SerializeField] private UnityEvent OnPsychicSelected;
    [SerializeField] private UnityEvent <string, bool> OnMoveSelected;
    [SerializeField] private UnityEvent OnFailedToMove;


    private void OnEnable()
    {
        MoveSelected = "";
        IsPsychicSelected = false;
    }

    public void SelectMove(string move)
    {
        // If we click the same button twice, we deselect the move.
        if (MoveSelected == move)
        {
            MoveSelected = "";
        }
        else
        {
            MoveSelected = move;
        }
        
        OnButtonSelected?.Invoke(move);
    }
    public void SelectPsychic()
    {
        IsPsychicSelected = (IsPsychicSelected) ? false : true;
        OnPsychicSelected?.Invoke();
    }

    public void EndTurn()
    {
        if (MoveSelected == "")
        {
            OnFailedToMove?.Invoke();
            Debug.Log("No move selected.");
            return;
        }
        // Debug.Log("Move: " + MoveSelected + " Psychic?: " + IsPsychicSelected);


        OnMoveSelected?.Invoke(MoveSelected, IsPsychicSelected);
        MoveSelected = "";
        IsPsychicSelected = false;

        gameObject.SetActive(false);
    }

    public void StartTurn()
    {
        gameObject.SetActive(true);
    }
    
}
