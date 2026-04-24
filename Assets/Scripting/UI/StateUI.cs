using TMPro;
using UnityEngine;

public class StateUI : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private bool HasTMP = false;

    private void Awake()
    {
        if (!TryGetComponent(out nameText)) Debug.Log("A NameUI could not find its TextMeshProUGUI.");
        else HasTMP = true;
    }

    public void UpdateState(TurnState newState)
    {
        if (HasTMP) nameText.text = newState.ToString();
    }
}
