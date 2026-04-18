using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
    private Image SelectionIndicator;
    private bool HasImage = false;
    [SerializeField] private string ButtonName;
    [SerializeField] private Color Selected;
    [SerializeField] private Color NotSelected;
    private bool IsSelected = false;

    private void Awake()
    {
        if (!TryGetComponent(out SelectionIndicator)) Debug.Log("A ButtonSelected component cannot find the image associated to it.");
        else HasImage = true;
    }

    public void Deselect()
    {
        if (HasImage)
        {
            SelectionIndicator.color = NotSelected;
            IsSelected = false;
        }
    }
    public void FlipSelection(string move)
    {
        if (HasImage)
        {
            if (move == ButtonName)
            {
                IsSelected = (IsSelected) ? false : true;
                SelectionIndicator.color = (IsSelected) ? Selected : NotSelected;
            }
            else
            {
                Deselect();
            }
        }
    }
}
