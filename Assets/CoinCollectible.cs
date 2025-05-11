using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    [Header("Effects")]
    public AudioClip collectSound;
    public GameObject collectEffect; // Optional particle effect
    
    [Header("Rotation")]
    public float rotationSpeed = 100f;
    
    [Header("Float Effect")]
    public bool enableFloating = true;
    public float floatSpeed = 2f;
    public float floatAmplitude = 0.5f;
    
    private Vector3 startPosition;
    private AudioSource audioSource;
    
     void Start()
    {
        startPosition = transform.position;
        
        // Add audio source if collect sound is assigned
        if (collectSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = collectSound;
        }
        
        // Ensure the coin is not affected by physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        
        // Make sure collider is a trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
    
    void Update()
    {
        // Rotate the coin
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Float up and down
        if (enableFloating)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check multiple ways to identify the player
        if (other.CompareTag("Player") || 
            other.GetComponent<character>() != null ||
            other.GetComponent<CharacterController>() != null ||
            other.GetComponentInParent<character>() != null)
        {
            CollectCoin();
        }
    }
    
    void CollectCoin()
    {
        Debug.Log("Coin collected!");
        
        // Update game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectCoin();
        }
        
        // Play collect sound - IMPORTANT: Play sound BEFORE destroying the coin
        if (collectSound != null)
        {
            // Use PlayClipAtPoint which works even after the GameObject is destroyed
            Debug.Log($"Playing sound {collectSound.name} at position {transform.position}");
            AudioSource.PlayClipAtPoint(collectSound, transform.position, 1.0f);
        }
        else
        {
            Debug.LogWarning("No collect sound assigned to coin!");
        }
        
        // Spawn effect if assigned
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
        
        // Destroy the coin
        Destroy(gameObject);
    }
}