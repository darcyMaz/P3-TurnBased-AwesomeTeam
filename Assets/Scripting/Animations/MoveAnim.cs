using UnityEngine;

public class MoveAnim : MonoBehaviour
{
    private Animator animator;
    private bool hasAnim = false;

    private void Awake()
    {
        if (!TryGetComponent(out animator)) Debug.Log("A MoveAnim component could not find its Animator.");
        else hasAnim = true;
    }

    public void TurnOnMoveAnim()
    {
        gameObject.SetActive(true);
        if (hasAnim)
        {
            animator.SetBool("IsMoving", true);
        }
    }
    public void TurnOffMoveAnim()
    {
        if (hasAnim)
        {
            animator.SetBool("IsMoving", false);
        }
        gameObject.SetActive(false);
    }
}
