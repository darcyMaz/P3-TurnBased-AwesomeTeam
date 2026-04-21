using UnityEngine;

public class StartUpPanel : MonoBehaviour
{
    public void OpenStartUpPanel()
    {
        gameObject.SetActive(true);
    }
    public void CloseStartUpPanel()
    {
        gameObject.SetActive(false);
    }
}
