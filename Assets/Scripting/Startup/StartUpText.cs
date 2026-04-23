using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StartUpText : MonoBehaviour
{
    private string[] StartUpDialogue;
    private int DialogueIndex = -1;
    [SerializeField] private int DialogueLength;

    private TextMeshProUGUI startUpText;
    private bool HasText = false;

    [SerializeField] private UnityEvent OnStartUpDone;

    private void Awake()
    {
        if (!TryGetComponent(out startUpText)) Debug.Log("The StartUpText component could not find its TextMeshPro.");
        else HasText = true;

        StartUpDialogue = new string[DialogueLength];

        StartUpDialogue[0] = "Welcome to the Super Battle Game!\r\n\r\nThis is a turn based game where the player (left) battles against enemies (on the right).";
        StartUpDialogue[1] = "You will face off against three enemies. Each one different from the last. \r\n You have 3 standard Moves and 1 special Move to choose from.";
        StartUpDialogue[2] = "You can choose 1 of the three standard moves on any given turn.\r\n\r\nAttack, Shield, Heal.";
        StartUpDialogue[3] = "You can ALSO choose to launch a Psychic attack at the same time as a standard move. \r\n\r\n But you only have three throughout the game, so use them wisely!";
        StartUpDialogue[4] = "A Psychic attack will likely weaken the opponent on that turn, but it's a gamble! It may also make them stronger.";
        StartUpDialogue[5] = "Ready to play? Press Next to start!";
    }

    public void Next()
    {
        if (!HasText) return;

        if (++DialogueIndex == DialogueLength)
        {
            OnStartUpDone?.Invoke();
            return;
        }
        startUpText.text = StartUpDialogue[DialogueIndex];
    }
}
