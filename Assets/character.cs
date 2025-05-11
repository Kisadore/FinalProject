using UnityEngine;
using LowPolyUnderwaterPack;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class character : MonoBehaviour
{
    [Header("Movement Settings")]
    public float landSpeed = 5f;
    public float swimSpeed = 3f;
    public float verticalSwimSpeed = 2f;
    public float gravity = -9.8f;
    public float buoyancy = 2.0f;
    
    [Header("Oxygen Settings")]
    public float oxygenDepletionRate = 1.0f;
    public float surfaceOxygenRefillRate = 5.0f;
    
    [Header("References")]
    public Transform playerModel;
    
    private CharacterController controller;
    private Camera playerCamera;
    private bool isUnderwater = false;
    private bool wasUnderwater = false;
    private Vector3 velocity = Vector3.zero;
    private float currentSpeed;
    
    // Water detection
    private WaterMesh currentWater;
    private UnderwaterEffect underwaterEffect;
    
    void Start()
    {
        Debug.Log("Character Start called");
        
        controller = GetComponent<CharacterController>();
        Debug.Log($"Controller found: {controller != null}");
        
        playerCamera = GetComponentInChildren<Camera>();
        Debug.Log($"Player camera found: {playerCamera != null}");
        
        if (playerCamera != null)
        {
            underwaterEffect = playerCamera.GetComponent<UnderwaterEffect>();
            Debug.Log($"Underwater effect found: {underwaterEffect != null}");
        }
        else
        {
            Debug.LogError("Player camera is null! Check if camera is child of character.");
        }
        
        // Disable CharacterController temporarily to set position
        if (controller != null)
            controller.enabled = false;
        
        // Set spawn position
        SetSpawnPosition();
        
        // Re-enable CharacterController
        if (controller != null)
            controller.enabled = true;
        
        // Start position monitoring
        StartCoroutine(MonitorPosition());
    }
    
    void SetSpawnPosition()
    {
        Debug.Log("SetSpawnPosition called");
        Debug.Log($"Current position before spawn: {transform.position}");
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            
            // Try to find GameManager in scene
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log($"Found GameManager in scene, difficulty: {gm.currentDifficulty}");
            }
            return;
        }
        
        Debug.Log($"GameManager found. Current difficulty: {GameManager.Instance.currentDifficulty}");
        
        if (GameManager.Instance.currentDifficulty == GameManager.GameDifficulty.Easy)
        {
            // Spawn on ship
            Vector3 shipPos = new Vector3(-88.0822f, -26.548f, -123.7431f);
            transform.position = shipPos;
            Debug.Log($"EASY MODE - Set position to ship: {shipPos}");
        }
        else if (GameManager.Instance.currentDifficulty == GameManager.GameDifficulty.Hard)
        {
            // Spawn underwater
            Vector3 underwaterPos = new Vector3(-117.129f, -45f, -116.1972f);
            transform.position = underwaterPos;
            Debug.Log($"HARD MODE - Set position to underwater: {underwaterPos}");
        }
        
        Debug.Log($"Final position after spawn: {transform.position}");
    }
    
    IEnumerator MonitorPosition()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log($"Position at {i * 0.1f}s: {transform.position}");
        }
    }
    
    void Update()
    {
        CheckWaterStatus();
        UpdateOxygen();
        
        // TEMPORARY DEBUG CONTROLS
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("F1 pressed - Forcing HARD mode spawn");
            
            // Disable controller, move, re-enable
            if (controller != null)
                controller.enabled = false;
                
            transform.position = new Vector3(-117.129f, -45f, -116.1972f);
            Debug.Log($"Forced position to: {transform.position}");
            
            if (controller != null)
                controller.enabled = true;
        }
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (GameManager.Instance != null)
            {
                Debug.Log($"F2 pressed - Current difficulty: {GameManager.Instance.currentDifficulty}");
            }
            else
            {
                Debug.Log("F2 pressed - GameManager is null!");
            }
            Debug.Log($"Current position: {transform.position}");
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("F3 pressed - Checking all GameManagers in scene");
            GameManager[] managers = FindObjectsOfType<GameManager>();
            Debug.Log($"Found {managers.Length} GameManager(s)");
            foreach (var gm in managers)
            {
                Debug.Log($"GameManager: {gm.name}, Difficulty: {gm.currentDifficulty}");
            }
        }
    }
    
    void CheckWaterStatus()
    {
        // Check if we're underwater using the UnderwaterEffect component
        if (underwaterEffect != null)
        {
            wasUnderwater = isUnderwater;
            isUnderwater = underwaterEffect.isUnderwater;
            
            // Transition effects
            if (isUnderwater && !wasUnderwater)
            {
                OnEnterWater();
            }
            else if (!isUnderwater && wasUnderwater)
            {
                OnExitWater();
            }
        }
        else
        {
            // Only use fallback detection in Hard mode to prevent false positives
            if (GameManager.Instance != null && GameManager.Instance.currentDifficulty == GameManager.GameDifficulty.Hard)
            {
                wasUnderwater = isUnderwater;
                
                // Fallback water detection using position
                if (transform.position.y < -30f) // Adjust this threshold
                {
                    isUnderwater = true;
                }
                else
                {
                    isUnderwater = false;
                }
                
                // Transition effects
                if (isUnderwater && !wasUnderwater)
                {
                    OnEnterWater();
                }
                else if (!isUnderwater && wasUnderwater)
                {
                    OnExitWater();
                }
            }
            else
            {
                // In Easy mode without underwater effect, assume not underwater
                isUnderwater = false;
            }
        }
        
        // Debug current water status
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log($"F4 pressed - IsUnderwater: {isUnderwater}");
            Debug.Log($"Position Y: {transform.position.y}");
            Debug.Log($"UnderwaterEffect exists: {underwaterEffect != null}");
            Debug.Log($"Current difficulty: {GameManager.Instance?.currentDifficulty}");
        }
    }
    
    void OnEnterWater()
    {
        Debug.Log("Entered water");
    }
    
    void OnExitWater()
    {
        Debug.Log("Exited water");
    }
    
    void UpdateOxygen()
    {
        if (GameManager.Instance == null || 
            GameManager.Instance.currentDifficulty != GameManager.GameDifficulty.Hard)
            return;
        
        if (isUnderwater)
        {
            // Deplete oxygen underwater
            GameManager.Instance.UpdateOxygen(oxygenDepletionRate * Time.deltaTime);
        }
        else
        {
            // Refill oxygen when not underwater (at surface)
            GameManager.Instance.RefillOxygen();
        }
    }
    
    public void MoveWithCC(Vector3 direction)
    {
        if (controller == null) return;
        
        if (isUnderwater)
        {
            HandleUnderwaterMovement(direction);
        }
        else
        {
            HandleLandMovement(direction);
        }
    }
    
//     void HandleUnderwaterMovement(Vector3 horizontalMovement)
// {
//     // Base movement from WASD
//     Vector3 finalMovement = horizontalMovement * swimSpeed;

//     // Check for vertical swim input
//     bool isSwimmingUp = Input.GetKey(KeyCode.Space);
//     bool isSwimmingDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

//     // Apply direct vertical movement
//     if (isSwimmingUp)
//     {
//         finalMovement.y = verticalSwimSpeed;
//         velocity.y = 0; // Cancel buoyancy
//         Debug.Log("Swimming UP");
//     }
//     else if (isSwimmingDown)
//     {
//         finalMovement.y = -verticalSwimSpeed;
//         velocity.y = 0; // Cancel buoyancy
//         Debug.Log("Swimming DOWN");
//     }
//     else
//     {
//         // Apply buoyancy when no vertical input
//         velocity.y += buoyancy * Time.deltaTime;
//         velocity.y = Mathf.Clamp(velocity.y, -verticalSwimSpeed, verticalSwimSpeed);
//         finalMovement.y = velocity.y;
//     }

//     // Apply drag to horizontal motion for water feel
//     finalMovement.x *= 0.9f;
//     finalMovement.z *= 0.9f;

//     // Debug info
//     Debug.Log($"Underwater Movement: {finalMovement}, Velocity Y: {velocity.y}");

//     // Move character
//     controller.Move(finalMovement * Time.deltaTime);

//     // Face direction (ignore Y to avoid vertical look)
//     if (horizontalMovement.magnitude > 0.1f)
//     {
//         Vector3 lookDirection = new Vector3(horizontalMovement.x, 0, horizontalMovement.z).normalized;
//         if (lookDirection != Vector3.zero)
//         {
//             transform.rotation = Quaternion.Slerp(transform.rotation,
//                 Quaternion.LookRotation(lookDirection), 10f * Time.deltaTime);
//         }
//     }
// }
void HandleUnderwaterMovement(Vector3 horizontalMovement)
    {
        // Base movement from WASD - increased speed for underwater
        Vector3 finalMovement = horizontalMovement * swimSpeed * 1.5f; // Increased multiplier

        // Check for vertical swim input
        bool isSwimmingUp = Input.GetKey(KeyCode.Space);
        bool isSwimmingDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        // Apply direct vertical movement
        if (isSwimmingUp)
        {
            finalMovement.y = verticalSwimSpeed;
            velocity.y = 0; // Cancel buoyancy
        }
        else if (isSwimmingDown)
        {
            finalMovement.y = -verticalSwimSpeed;
            velocity.y = 0; // Cancel buoyancy
        }
        else
        {
            // Apply buoyancy when no vertical input
            velocity.y += buoyancy * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, -verticalSwimSpeed, verticalSwimSpeed);
            finalMovement.y = velocity.y;
        }

        // Remove or reduce drag for better responsiveness
        // finalMovement.x *= 0.9f; // Removed drag
        // finalMovement.z *= 0.9f; // Removed drag

        // Move character
        controller.Move(finalMovement * Time.deltaTime);

        // Face direction (ignore Y to avoid vertical look)
        if (horizontalMovement.magnitude > 0.1f)
        {
            Vector3 lookDirection = new Vector3(horizontalMovement.x, 0, horizontalMovement.z).normalized;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(lookDirection), 10f * Time.deltaTime);
            }
        }
    }
    
    void HandleLandMovement(Vector3 horizontalMovement)
    {
        Vector3 finalMovement = horizontalMovement * landSpeed;
        
        // Apply gravity on land
        if (controller.isGrounded)
        {
            velocity.y = -2f; // Small downward force to keep grounded
            
            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float jumpHeight = 2f;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        
        finalMovement.y = velocity.y;
        controller.Move(finalMovement * Time.deltaTime);
        
        // Rotate character to face movement direction
        if (horizontalMovement.magnitude > 0.1f)
        {
            Vector3 lookDirection = new Vector3(horizontalMovement.x, 0, horizontalMovement.z).normalized;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    Quaternion.LookRotation(lookDirection), 10f * Time.deltaTime);
            }
        }
    }
    
    public bool IsUnderwater() => isUnderwater;
}