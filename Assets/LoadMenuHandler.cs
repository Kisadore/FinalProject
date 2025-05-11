using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadMenuHandler : MonoBehaviour
{
    [Header("UI References")]
    public Button[] saveSlotButtons;
    public Button backButton;
    public MainMenuManager mainMenuManager;
    
    void Start()
    {
        // Set up back button
        if (backButton != null && mainMenuManager != null)
        {
            backButton.onClick.AddListener(() => mainMenuManager.ReturnToMainMenu());
        }
        
        // Set up save slot buttons if they exist
        if (saveSlotButtons != null)
        {
            for (int i = 0; i < saveSlotButtons.Length; i++)
            {
                int slotIndex = i;
                saveSlotButtons[i].onClick.AddListener(() => LoadGame(slotIndex));
                UpdateSlotButton(i);
            }
        }
    }
    
    public void OpenLoadWindow()
    {
        Debug.Log("LoadMenuHandler.OpenLoadWindow called");
        gameObject.SetActive(true);
    }
    
    public void CloseLoadWindow()
    {
        Debug.Log("LoadMenuHandler.CloseLoadWindow called");
        gameObject.SetActive(false);
    }
    
    void UpdateSlotButton(int slotIndex)
    {
        if (saveSlotButtons == null || slotIndex >= saveSlotButtons.Length)
            return;
            
        string saveKey = $"SaveSlot_{slotIndex}";
        bool saveExists = PlayerPrefs.HasKey(saveKey + "_exists");
        
        TextMeshProUGUI buttonText = saveSlotButtons[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            if (saveExists)
            {
                string saveDate = PlayerPrefs.GetString(saveKey + "_date", "Unknown Date");
                buttonText.text = $"Slot {slotIndex + 1} - {saveDate}";
            }
            else
            {
                buttonText.text = $"Slot {slotIndex + 1} - Empty";
            }
        }
        
        saveSlotButtons[slotIndex].interactable = saveExists;
    }
    
    void LoadGame(int slotIndex)
    {
        Debug.Log($"Loading game from slot {slotIndex}");
        // Add your actual load game logic here
    }
}