using UnityEngine;

public class PowerupCollectible : MonoBehaviour
{
    public PowerupManager.PowerupType powerupType = PowerupManager.PowerupType.Random;
    public float rotationSpeed = 50f;
    public float bobAmplitude = 0.2f;
    public float bobFrequency = 1f;
    
    private Vector3 startPosition;
    private float bobTime;
    
    private void Start()
    {
        startPosition = transform.position;
        bobTime = Random.Range(0f, 2f * Mathf.PI); // Random start point in the cycle
        
        // Assign a visual based on the powerup type
        UpdateVisual();
    }
    
    private void Update()
    {
        // Rotate the powerup
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Make the powerup bob up and down
        bobTime += Time.deltaTime * bobFrequency;
        Vector3 offset = Vector3.up * Mathf.Sin(bobTime) * bobAmplitude;
        transform.position = startPosition + offset;
    }
    
    private void UpdateVisual()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite == null) return;
        
        // Assign different colors based on powerup type
        switch (powerupType)
        {
            case PowerupManager.PowerupType.Shield:
                sprite.color = new Color(0.2f, 0.5f, 1f); // Blue
                break;
                
            case PowerupManager.PowerupType.SpeedBoost:
                sprite.color = new Color(1f, 0.8f, 0.2f); // Yellow
                break;
                
            case PowerupManager.PowerupType.ScoreMultiplier:
                sprite.color = new Color(0.6f, 0.2f, 1f); // Purple
                break;
                
            case PowerupManager.PowerupType.ExtraLife:
                sprite.color = new Color(1f, 0.2f, 0.2f); // Red
                break;
                
            case PowerupManager.PowerupType.Random:
                sprite.color = new Color(0.2f, 1f, 0.5f); // Green
                break;
        }
    }
} 