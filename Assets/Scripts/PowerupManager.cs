using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    [Header("Powerup Settings")]
    public float shieldDuration = 10f;
    public float speedBoostDuration = 8f;
    public float speedBoostMultiplier = 1.5f;
    public float scoreMultiplierDuration = 15f;
    public float scoreMultiplierValue = 2f;
    
    [Header("UI References")]
    public GameObject shieldIcon;
    public GameObject speedBoostIcon;
    public GameObject scoreMultiplierIcon;
    public Text powerupTimerText;
    
    // References
    private PlayerMovement playerMovement;
    private GameManager gameManager;
    
    // Active powerups
    private bool shieldActive = false;
    private bool speedBoostActive = false;
    private bool scoreMultiplierActive = false;
    
    // Timers
    private float shieldTimer = 0f;
    private float speedBoostTimer = 0f;
    private float scoreMultiplierTimer = 0f;
    
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        gameManager = FindObjectOfType<GameManager>();
        
        // Disable all powerup icons
        if (shieldIcon != null) shieldIcon.SetActive(false);
        if (speedBoostIcon != null) speedBoostIcon.SetActive(false);
        if (scoreMultiplierIcon != null) scoreMultiplierIcon.SetActive(false);
        if (powerupTimerText != null) powerupTimerText.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        // Update active powerup timers
        UpdatePowerupTimers();
    }
    
    public void ActivatePowerup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Shield:
                ActivateShield();
                break;
            case PowerupType.SpeedBoost:
                ActivateSpeedBoost();
                break;
            case PowerupType.ScoreMultiplier:
                ActivateScoreMultiplier();
                break;
            case PowerupType.ExtraLife:
                ActivateExtraLife();
                break;
            case PowerupType.Random:
                ActivateRandomPowerup();
                break;
        }
    }
    
    private void ActivateShield()
    {
        shieldActive = true;
        shieldTimer = shieldDuration;
        
        if (shieldIcon != null) shieldIcon.SetActive(true);
        if (powerupTimerText != null)
        {
            powerupTimerText.gameObject.SetActive(true);
            powerupTimerText.text = "Shield: " + Mathf.CeilToInt(shieldTimer).ToString();
        }
        
        // Apply effect to player
        if (playerMovement != null)
        {
            playerMovement.SetInvulnerable(true);
            
            // Visual effect for shield (example: change player color)
            SpriteRenderer playerSprite = playerMovement.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                playerSprite.color = new Color(0.7f, 0.7f, 1.0f, 0.9f);
            }
        }
    }
    
    private void ActivateSpeedBoost()
    {
        speedBoostActive = true;
        speedBoostTimer = speedBoostDuration;
        
        if (speedBoostIcon != null) speedBoostIcon.SetActive(true);
        if (powerupTimerText != null)
        {
            powerupTimerText.gameObject.SetActive(true);
            powerupTimerText.text = "Speed Boost: " + Mathf.CeilToInt(speedBoostTimer).ToString();
        }
        
        // Apply effect to player
        if (playerMovement != null)
        {
            // Store original speed before boost
            float originalSpeed = playerMovement.speed;
            playerMovement.speed = originalSpeed * speedBoostMultiplier;
            
            // Visual effect for speed boost
            TrailRenderer trail = playerMovement.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                trail.emitting = true;
            }
        }
    }
    
    private void ActivateScoreMultiplier()
    {
        scoreMultiplierActive = true;
        scoreMultiplierTimer = scoreMultiplierDuration;
        
        if (scoreMultiplierIcon != null) scoreMultiplierIcon.SetActive(true);
        if (powerupTimerText != null)
        {
            powerupTimerText.gameObject.SetActive(true);
            powerupTimerText.text = "Score x" + scoreMultiplierValue + ": " + Mathf.CeilToInt(scoreMultiplierTimer).ToString();
        }
        
        // Apply effect to game manager
        if (gameManager != null)
        {
            gameManager.SetScoreMultiplier(scoreMultiplierValue);
        }
    }
    
    private void ActivateExtraLife()
    {
        // Apply effect to player
        if (playerMovement != null)
        {
            playerMovement.AddLife();
        }
        
        // Show a quick text notification
        if (powerupTimerText != null)
        {
            powerupTimerText.gameObject.SetActive(true);
            powerupTimerText.text = "Extra Life!";
            StartCoroutine(HideTimerTextAfterDelay(2f));
        }
    }
    
    private void ActivateRandomPowerup()
    {
        // Choose a random powerup type excluding Random
        PowerupType[] types = { 
            PowerupType.Shield, 
            PowerupType.SpeedBoost, 
            PowerupType.ScoreMultiplier, 
            PowerupType.ExtraLife 
        };
        
        PowerupType randomType = types[Random.Range(0, types.Length)];
        ActivatePowerup(randomType);
    }
    
    private void UpdatePowerupTimers()
    {
        // Update shield timer
        if (shieldActive)
        {
            shieldTimer -= Time.deltaTime;
            
            // Update timer text if this is the active powerup being displayed
            if (powerupTimerText != null && powerupTimerText.text.StartsWith("Shield"))
            {
                powerupTimerText.text = "Shield: " + Mathf.CeilToInt(shieldTimer).ToString();
            }
            
            if (shieldTimer <= 0)
            {
                DeactivateShield();
            }
        }
        
        // Update speed boost timer
        if (speedBoostActive)
        {
            speedBoostTimer -= Time.deltaTime;
            
            // Update timer text if this is the active powerup being displayed
            if (powerupTimerText != null && powerupTimerText.text.StartsWith("Speed"))
            {
                powerupTimerText.text = "Speed Boost: " + Mathf.CeilToInt(speedBoostTimer).ToString();
            }
            
            if (speedBoostTimer <= 0)
            {
                DeactivateSpeedBoost();
            }
        }
        
        // Update score multiplier timer
        if (scoreMultiplierActive)
        {
            scoreMultiplierTimer -= Time.deltaTime;
            
            // Update timer text if this is the active powerup being displayed
            if (powerupTimerText != null && powerupTimerText.text.StartsWith("Score"))
            {
                powerupTimerText.text = "Score x" + scoreMultiplierValue + ": " + Mathf.CeilToInt(scoreMultiplierTimer).ToString();
            }
            
            if (scoreMultiplierTimer <= 0)
            {
                DeactivateScoreMultiplier();
            }
        }
    }
    
    private void DeactivateShield()
    {
        shieldActive = false;
        if (shieldIcon != null) shieldIcon.SetActive(false);
        
        // Remove effect from player
        if (playerMovement != null)
        {
            playerMovement.SetInvulnerable(false);
            
            // Reset player color
            SpriteRenderer playerSprite = playerMovement.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                playerSprite.color = Color.white;
            }
        }
        
        // Hide timer text if no other powerups are active
        if (!speedBoostActive && !scoreMultiplierActive && powerupTimerText != null)
        {
            powerupTimerText.gameObject.SetActive(false);
        }
    }
    
    private void DeactivateSpeedBoost()
    {
        speedBoostActive = false;
        if (speedBoostIcon != null) speedBoostIcon.SetActive(false);
        
        // Remove effect from player
        if (playerMovement != null)
        {
            // Reset to original speed
            playerMovement.ResetSpeed();
            
            // Disable trail
            TrailRenderer trail = playerMovement.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                trail.emitting = false;
            }
        }
        
        // Hide timer text if no other powerups are active
        if (!shieldActive && !scoreMultiplierActive && powerupTimerText != null)
        {
            powerupTimerText.gameObject.SetActive(false);
        }
    }
    
    private void DeactivateScoreMultiplier()
    {
        scoreMultiplierActive = false;
        if (scoreMultiplierIcon != null) scoreMultiplierIcon.SetActive(false);
        
        // Remove effect from game manager
        if (gameManager != null)
        {
            gameManager.ResetScoreMultiplier();
        }
        
        // Hide timer text if no other powerups are active
        if (!shieldActive && !speedBoostActive && powerupTimerText != null)
        {
            powerupTimerText.gameObject.SetActive(false);
        }
    }
    
    private IEnumerator HideTimerTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Only hide if no other powerups are active
        if (!shieldActive && !speedBoostActive && !scoreMultiplierActive && powerupTimerText != null)
        {
            powerupTimerText.gameObject.SetActive(false);
        }
    }
    
    public enum PowerupType
    {
        Shield,
        SpeedBoost,
        ScoreMultiplier,
        ExtraLife,
        Random
    }
} 