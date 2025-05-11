using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI coinsText;
    public Slider oxygenSlider;
    public GameObject oxygenUI; // Only shown in hard mode
    
    void Start()
    {
        Debug.Log("GameUI Start called");
        StartCoroutine(InitializeUI());
    }
    
    IEnumerator InitializeUI()
    {
        // Wait for GameManager to be ready
        yield return new WaitForSeconds(0.1f);
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameUI: GameManager.Instance is null!");
            yield break;
        }
        
        Debug.Log($"GameUI: Current difficulty is {GameManager.Instance.currentDifficulty}");
        
        // Show/hide oxygen UI based on difficulty
        if (GameManager.Instance.currentDifficulty == GameManager.GameDifficulty.Easy)
        {
            if (oxygenUI != null)
            {
                oxygenUI.SetActive(false);
                Debug.Log("GameUI: Hiding oxygen UI for Easy mode");
            }
        }
        else
        {
            if (oxygenUI != null)
            {
                oxygenUI.SetActive(true);
                Debug.Log("GameUI: Showing oxygen UI for Hard mode");
            }
            else
            {
                Debug.LogError("GameUI: oxygenUI reference is null!");
            }
        }
        
        // Check if oxygen slider is connected
        if (oxygenSlider == null)
        {
            Debug.LogError("GameUI: oxygenSlider reference is null!");
        }
        else
        {
            Debug.Log("GameUI: oxygenSlider is connected");
        }
    }
    
    void Update()
    {
        if (GameManager.Instance == null) return;
        
        // Update time display
        float timeRemaining = GameManager.Instance.GetTimeRemaining();
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        
        if (timeText != null)
        {
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            // Flash time when critical (1 minute or less)
            if (timeRemaining <= 60f)
            {
                timeText.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 2f, 1f));
                
                // Make text bigger when critical
                timeText.fontSize = Mathf.Lerp(24f, 32f, Mathf.PingPong(Time.time, 1f));
            }
            else
            {
                timeText.color = Color.white;
                timeText.fontSize = 24f; // Reset to normal size
            }
        }
        
        // Update coins display
        if (coinsText != null)
        {
            coinsText.text = $"Coins: {GameManager.Instance.coinsCollected}/{GameManager.Instance.coinsRequired}";
        }
        
        // Update oxygen display (hard mode only)
        if (GameManager.Instance.currentDifficulty == GameManager.GameDifficulty.Hard)
        {
            if (oxygenSlider != null && oxygenUI != null && oxygenUI.activeSelf)
            {
                float oxygenPercentage = GameManager.Instance.GetOxygenPercentage();
                oxygenSlider.value = oxygenPercentage;
                
                // Flash oxygen bar when critical (1 minute or less)
                float oxygenRemaining = GameManager.Instance.GetOxygenRemaining();
                if (oxygenRemaining <= 60f)
                {
                    var sliderFill = oxygenSlider.fillRect.GetComponent<Image>();
                    if (sliderFill != null)
                    {
                        sliderFill.color = Color.Lerp(Color.cyan, Color.red, Mathf.PingPong(Time.time * 2f, 1f));
                    }
                }
            }
        }
    }
}