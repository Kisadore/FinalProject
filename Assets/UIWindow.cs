using UnityEngine;

public class UIWindow : MonoBehaviour
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    
    public void OpenWindow()
    {

        gameObject.SetActive(true);
        

        if (canvas != null)
        {
            canvas.enabled = true;
        }
        

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void CloseWindow()
    {
        if (canvas != null)
        {
            canvas.enabled = false;
        }

    }
}