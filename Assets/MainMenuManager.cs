using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Windows")]
    public UIWindow mainMenuWindow;
    public DifficultyMenu difficultyMenu;
    public UIWindow settingsWindow;
    public LoadMenuHandler loadMenu; // Changed from LoadMenu to LoadMenuHandler
    
    [Header("Main Menu Buttons")]
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button quitButton;
    
    void Start()
    {
        // Set up button listeners
        newGameButton.onClick.AddListener(OpenDifficultySelection);
        loadGameButton.onClick.AddListener(OpenLoadMenu);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        
        // Show main menu on start
        ShowMainMenu();
    }
    
    void ShowMainMenu()
    {
        mainMenuWindow.OpenWindow();
        difficultyMenu.CloseDifficultyWindow();
        settingsWindow.CloseWindow();
        loadMenu.CloseLoadWindow();
    }
    
    void OpenDifficultySelection()
    {
        mainMenuWindow.CloseWindow();
        difficultyMenu.OpenDifficultyWindow();
    }
    
    void OpenLoadMenu()
    {
        Debug.Log("MainMenuManager.OpenLoadMenu called");
        mainMenuWindow.CloseWindow();
        loadMenu.OpenLoadWindow();
    }
    
    void OpenSettings()
    {
        mainMenuWindow.CloseWindow();
        settingsWindow.OpenWindow();
    }
    
    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    // Called by back buttons in other menus
    public void ReturnToMainMenu()
    {
        ShowMainMenu();
    }
}