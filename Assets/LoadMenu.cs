using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    public void OpenLoadWindow()
    {
        gameObject.SetActive(true);
    }

    public void CloseLoadWindow()
    {
        gameObject.SetActive(false);
    }
}