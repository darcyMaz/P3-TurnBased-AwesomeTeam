using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
    protected Image SelectionIndicator;
    private bool HasImage = false;
    [SerializeField] private string ButtonName;
    [SerializeField] protected Color Selected;
    [SerializeField] protected Color NotSelected;
    private bool IsSelected = false;

    private void Awake()
    {
        if (!TryGetComponent(out SelectionIndicator)) Debug.Log("A ButtonSelected component cannot find the image associated to it.");
        else HasImage = true;

        // Debug.Log(HasImage);
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
            // Debug.Log(move + " here is the problem, it sends in empty or a move it can't have. So i need to have a diff func in movepanel called psychic pressed don't i have that?");

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
