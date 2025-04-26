using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float initialSpeed = 5f;
    public float maxSpeed = 15f;
    public float speedIncreaseRate = 0.1f;
    public int scoreThresholdForEraChange = 500;
    public int scoreThresholdForPuzzle = 300;
    
    [Header("UI References")]
    public Text scoreText;
    public Text highScoreText;
    public Text currentEraText;
    public GameObject puzzlePanel;
    
    [Header("Era Settings")]
    public List<EraSetting> eras;
    
    // Private variables
    private int currentScore = 0;
    private int highScore = 0;
    private int currentEraIndex = 0;
    private bool puzzleCompleted = false;
    private bool gameOver = false;
    private PlayerMovement playerMovement;
    private ObstacleGenerator obstacleGenerator;
    private EndlessGround groundController;
    private PuzzleManager puzzleManager;
    private float scoreMultiplier = 1f;
    
    [System.Serializable]
    public class EraSetting
    {
        public string eraName;
        public Color backgroundColor;
        public GameObject uniqueObstaclePrefab;
        public Sprite backgroundSprite;
        public PuzzleType puzzleType;
    }
    
    public enum PuzzleType
    {
        MemoryMatch,
        QuickTimeEvent,
        SimpleQuiz,
        PatternSequence
    }
    
    private void Start()
    {
        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        // Update high score text
        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
        
        // Get references to other components
        playerMovement = FindObjectOfType<PlayerMovement>();
        obstacleGenerator = FindObjectOfType<ObstacleGenerator>();
        groundController = FindObjectOfType<EndlessGround>();
        puzzleManager = FindObjectOfType<PuzzleManager>();
        
        // Set initial era
        if (eras.Count > 0 && currentEraText != null)
        {
            currentEraText.text = "Current Era: " + eras[currentEraIndex].eraName;
            Camera.main.backgroundColor = eras[currentEraIndex].backgroundColor;
        }
        
        // Set initial speed
        if (playerMovement != null)
            playerMovement.speed = initialSpeed;
        
        if (groundController != null)
            groundController.speed = initialSpeed;
        
        // Start score incrementing
        StartCoroutine(IncrementScore());
    }
    
    private IEnumerator IncrementScore()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(0.1f);
            currentScore += Mathf.RoundToInt(1 * scoreMultiplier);
            
            // Update score text
            if (scoreText != null)
                scoreText.text = "Score: " + currentScore;
            
            // Check for era transition
            if (currentScore > 0 && currentScore % scoreThresholdForEraChange == 0)
            {
                ChangeToPuzzle();
            }
            
            // Increase game speed
            if (playerMovement.speed < maxSpeed)
            {
                float newSpeed = initialSpeed + (currentScore / 100f * speedIncreaseRate);
                newSpeed = Mathf.Min(newSpeed, maxSpeed);
                
                // Update speeds
                playerMovement.speed = newSpeed;
                groundController.speed = newSpeed;
                obstacleGenerator.spawnRate = Mathf.Max(0.5f, 2f - (currentScore / 500f));
            }
        }
    }
    
    public void TriggerEraPuzzle()
    {
        ChangeToPuzzle();
    }
    
    private void ChangeToPuzzle()
    {
        // Pause the game
        Time.timeScale = 0;
        
        // Show puzzle panel
        if (puzzlePanel != null)
            puzzlePanel.SetActive(true);
        
        // Check if we have a puzzle manager to handle the puzzle
        if (puzzleManager != null)
        {
            // Start the appropriate puzzle for this era
            PuzzleType puzzleType = (eras.Count > 0) ? eras[currentEraIndex].puzzleType : PuzzleType.MemoryMatch;
            puzzleManager.StartPuzzle(puzzleType, currentEraIndex);
        }
        else
        {
            // If no puzzle manager, just simulate puzzle completion
            StartCoroutine(SimulatePuzzle());
        }
    }
    
    // Method called by PuzzleManager when puzzle is completed
    public void PuzzleCompleted(bool success)
    {
        puzzleCompleted = success;
        
        if (puzzleCompleted)
        {
            Debug.Log("Puzzle completed! Powerup granted.");
            // Give player powerup
            PowerupManager powerupManager = FindObjectOfType<PowerupManager>();
            if (powerupManager != null)
            {
                powerupManager.ActivatePowerup(PowerupManager.PowerupType.Random);
            }
        }
        
        // Move to next era
        currentEraIndex = (currentEraIndex + 1) % eras.Count;
        
        // Update era text
        if (currentEraText != null)
            currentEraText.text = "Current Era: " + eras[currentEraIndex].eraName;
        
        // Change background color
        Camera.main.backgroundColor = eras[currentEraIndex].backgroundColor;
        
        // Update obstacle prefab if this era has a unique one
        if (eras[currentEraIndex].uniqueObstaclePrefab != null && obstacleGenerator != null)
        {
            obstacleGenerator.obstaclePrefab = eras[currentEraIndex].uniqueObstaclePrefab;
        }
        
        // Hide puzzle panel
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);
        
        // Resume game
        Time.timeScale = 1;
    }
    
    private IEnumerator SimulatePuzzle()
    {
        // Wait for 3 seconds to simulate puzzle time
        float puzzleTime = 3f;
        while (puzzleTime > 0)
        {
            puzzleTime -= Time.unscaledDeltaTime;
            yield return null;
        }
        
        // 50% chance to complete the puzzle
        puzzleCompleted = UnityEngine.Random.value > 0.5f;
        
        PuzzleCompleted(puzzleCompleted);
    }
    
    public void GameOver()
    {
        gameOver = true;
        
        // Update high score if needed
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            
            if (highScoreText != null)
                highScoreText.text = "High Score: " + highScore;
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Powerup methods
    public void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
    }
    
    public void ResetScoreMultiplier()
    {
        scoreMultiplier = 1f;
    }
} 