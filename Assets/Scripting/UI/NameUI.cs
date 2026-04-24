using TMPro;
using UnityEngine;

public class NameUI : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private bool HasTMP = false;

    private void Awake()
    {
        if (!TryGetComponent(out nameText)) Debug.Log("A NameUI could not find its TextMeshProUGUI.");
        else HasTMP = true;
    }

    public void UpdateName(string newName)
    {
        if (HasTMP) nameText.text = newName;
    }
}
