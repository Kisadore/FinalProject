using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public character playerCharacter;
    public Transform cameraTransform;
    public Transform cameraPivotTransform; 
    public float cameraPivotSpeed = 100f;
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

//     void Update()
//     {
//         // Debug check if the script is enabled
//         if (Input.GetKeyDown(KeyCode.F5))
//         {
//             Debug.Log($"PlayerInputHandler is enabled: {enabled}");
//             Debug.Log($"Game is active: {GameManager.Instance?.IsGameActive()}");
//             Debug.Log($"Time remaining: {GameManager.Instance?.GetTimeRemaining()}");
//             Debug.Log($"Oxygen remaining: {GameManager.Instance?.GetOxygenRemaining()}");
//         }
        
//         Vector3 cameraForward = cameraTransform.forward;
//         Vector3 cameraRight = cameraTransform.right;
//         Vector3 finalMovement = Vector3.zero;
        
//         // For underwater movement, keep the Y component
//         if (!playerCharacter.IsUnderwater())
//         {
//             // On land, remove Y component for horizontal movement only
//             cameraForward.y = 0;
//             cameraRight.y = 0;
//             cameraForward.Normalize();
//             cameraRight.Normalize();
//         }

//         // WASD movement
//         if (Input.GetKey(KeyCode.W))
//         {
//             finalMovement += cameraForward;
//         }
//         if (Input.GetKey(KeyCode.S))
//         {
//             finalMovement -= cameraForward;
//         }
//         if (Input.GetKey(KeyCode.A))
//         {
//             finalMovement -= cameraRight;
//         }
//         if (Input.GetKey(KeyCode.D))
//         {
//             finalMovement += cameraRight;
//         }

//         // Only normalize horizontal movement, not when including vertical
//         if (!playerCharacter.IsUnderwater())
//         {
//             if (finalMovement != Vector3.zero)
//             {
//                 finalMovement.Normalize();
//             }
//         }
//         else if (finalMovement != Vector3.zero)
//         {
//             // For underwater, normalize but preserve the magnitude for diagonal movement
//             finalMovement = finalMovement.normalized;
//         }
        
//         // Pass movement to character controller
//         playerCharacter.MoveWithCC(finalMovement);
        
//         // Optional camera rotation controls
//         if (Input.GetKey(KeyCode.Q))
//         {
//             cameraPivotTransform.Rotate(new Vector3(0, 1, 0), -cameraPivotSpeed * Time.deltaTime);
//         }
//         if (Input.GetKey(KeyCode.E))
//         {
//             cameraPivotTransform.Rotate(Vector3.up, cameraPivotSpeed * Time.deltaTime);
//         }
//     }
// }
void Update()
    {
        // Don't process input if game is paused
        var pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.IsPaused())
        {
            return;
        }
        
        // Debug check if the script is enabled
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log($"PlayerInputHandler is enabled: {enabled}");
            Debug.Log($"Game is active: {GameManager.Instance?.IsGameActive()}");
            Debug.Log($"Time remaining: {GameManager.Instance?.GetTimeRemaining()}");
            Debug.Log($"Oxygen remaining: {GameManager.Instance?.GetOxygenRemaining()}");
        }
        
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        Vector3 finalMovement = Vector3.zero;
        
        // For underwater movement, keep the Y component
        if (!playerCharacter.IsUnderwater())
        {
            // On land, remove Y component for horizontal movement only
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();
        }

        // WASD movement
        if (Input.GetKey(KeyCode.W))
        {
            finalMovement += cameraForward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            finalMovement -= cameraForward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            finalMovement -= cameraRight;
        }
        if (Input.GetKey(KeyCode.D))
        {
            finalMovement += cameraRight;
        }

        // Only normalize horizontal movement, not when including vertical
        if (!playerCharacter.IsUnderwater())
        {
            if (finalMovement != Vector3.zero)
            {
                finalMovement.Normalize();
            }
        }
        else if (finalMovement != Vector3.zero)
        {
            // For underwater, normalize but preserve the magnitude for diagonal movement
            finalMovement = finalMovement.normalized;
        }
        
        // Pass movement to character controller
        playerCharacter.MoveWithCC(finalMovement);
        
        // Optional camera rotation controls
        if (Input.GetKey(KeyCode.Q))
        {
            cameraPivotTransform.Rotate(new Vector3(0, 1, 0), -cameraPivotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            cameraPivotTransform.Rotate(Vector3.up, cameraPivotSpeed * Time.deltaTime);
        }
    }
}