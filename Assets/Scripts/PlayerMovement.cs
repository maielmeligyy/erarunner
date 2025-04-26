using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public int lives = 4;
    public GameObject[] lifeIcons;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            lives--;
            Debug.Log("Hit obstacle! Lives left: " + lives);
            Destroy(collision.gameObject);
            if (lives >= 0 && lives < lifeIcons.Length)
            {
                lifeIcons[lives].SetActive(false); // Hide one life icon
            }

            if (lives <= 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        this.enabled = false;

        GameObject gameOverPanel = GameObject.Find("GameOverPanel");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("Game Over");
    }
}
