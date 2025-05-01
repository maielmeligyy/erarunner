using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public int lives = 4;
    public GameObject[] lifeIcons;
    public int score = 0;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDead = false;
    private bool isInvulnerable = false;
    private float originalSpeed;
    private GameManager gameManager;
    private int shieldHitsRemaining = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        gameManager = FindObjectOfType<GameManager>();
    }
    public void ActivateShield(int hits)
    {
        shieldHitsRemaining = hits;
        isInvulnerable = true;
        Debug.Log("Shield activated for " + hits + " obstacle(s).");
    }
    private void Update()
    {
        if (!isDead)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        }
    }

    public void Jump()
    {
        if (isGrounded && !isDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("Jump button pressed");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isDead)
        {
            if (isInvulnerable)
            {
                shieldHitsRemaining--;
                Debug.Log("Shield absorbed a hit! Remaining: " + shieldHitsRemaining);

                if (shieldHitsRemaining <= 0)
                {
                    isInvulnerable = false;
                    Debug.Log("Shield expired.");
                }

                Destroy(collision.gameObject);
            }
            else
            {
                lives--;
                Debug.Log("Hit obstacle! Lives left: " + lives);
                Destroy(collision.gameObject);

                if (lives >= 0 && lives < lifeIcons.Length)
                {
                    lifeIcons[lives].SetActive(false);
                }

                if (lives <= 0)
                {
                    Destroy(gameObject);
                    GameOver();
                }
            }
        }

        if (collision.gameObject.CompareTag("Coin") && !isDead)
        {

            Debug.Log("Coin collected!");
            Destroy(collision.gameObject);

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(1);
            }
            else
            {
                Debug.LogWarning("ScoreManager is missing in scene.");
            }


        }
        else if (collision.gameObject.CompareTag("Obstacle") && isInvulnerable)
        {
            // If player has shield, just destroy the obstacle without losing life
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Collectible"))
        {
            // Handle collectible pickup (era transition triggers)
            Destroy(collision.gameObject);
            if (gameManager != null)
            {
                gameManager.TriggerEraPuzzle();
            }
        }
        else if (collision.gameObject.CompareTag("Powerup"))
        {
            // Determine powerup type from the collectible
            PowerupManager.PowerupType type = PowerupManager.PowerupType.Random;
            
            PowerupCollectible powerupCollectible = collision.gameObject.GetComponent<PowerupCollectible>();
            if (powerupCollectible != null)
            {
                type = powerupCollectible.powerupType;
            }
            
            // Activate the powerup
            PowerupManager powerupManager = FindObjectOfType<PowerupManager>();
            if (powerupManager != null)
            {
                powerupManager.ActivatePowerup(type);
            }
            
            // Destroy the powerup object
            Destroy(collision.gameObject);
        }
    }

    // Called when player runs out of lives
    private void GameOver()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ShowGameOver();
        }

        

        Debug.Log("Game Over");
    }
    
    // For powerups
    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }
    
    public void ResetSpeed()
    {
        speed = originalSpeed;
    }
    
    public void AddLife()
    {
        // Only add life if not at max
        if (lives < lifeIcons.Length)
        {
            lives++;
            lifeIcons[lives-1].SetActive(true);
        }
    }
}
