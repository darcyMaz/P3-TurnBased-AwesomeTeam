using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
    private Image SelectionIndicator;
    [SerializeField] private Color Selected;
    [SerializeField] private Color NotSelected;
    private bool IsSelected = false;



    public void Deselect()
    {
        SelectionIndicator.color = NotSelected;
        IsSelected = false;
    }
    public void FlipSelection()
    {
        if (IsSelected)
        {
            SelectionIndicator.color = Selected;
        }
        else
        {
            SelectionIndicator.color = NotSelected;
        }
    }
}
