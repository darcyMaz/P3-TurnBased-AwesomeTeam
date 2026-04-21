using UnityEngine;

public class PsychicBar : ButtonSelected
{
    [SerializeField] private int PsychicBarNumber;
    [SerializeField] private Color Consumed;

    public void CheckIfConsumed(int TotalPsyAttacksLeft)
    {
        // Debug.Log(TotalPsyAttacksLeft + " " + name);

        // This means that we've consumed this psy attack and need to grey out the image.
        if (TotalPsyAttacksLeft < PsychicBarNumber)
        {
            Selected = Consumed;
            NotSelected = Consumed;
            SelectionIndicator.color = Consumed;
        }
    }
}
