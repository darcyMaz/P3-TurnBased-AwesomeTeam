using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    private bool HasTMP = false;

    private void Awake()
    {
        if (!TryGetComponent(out healthText)) Debug.Log("A HealthUI could not find its TextMeshProUGUI.");
        else HasTMP = true;
    }

    public void UpdateHealth(int health)
    {
        if (HasTMP) healthText.text = "Health: " + health.ToString();
    }
}
