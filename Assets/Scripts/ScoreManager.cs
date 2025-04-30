using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;
    public GameObject level2Background; // assign in Inspector
    public GameObject level1Background;
    public static ScoreManager Instance;
    public GameObject mcqPanel;
    public bool secondlevel = false;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void EnterSecondLevel()
    {
        level1Background.SetActive(false);
        level2Background.SetActive(true);
        secondlevel = true;
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.ActivateShield(2);
            score = 0;// give shield that lasts for 2 hits
        }
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Score: " + score.ToString();
    }
    
public void ReplayGame()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score increased to: " + score);
        UpdateScoreUI();
        if (score == 5 && !secondlevel)
        {
            Time.timeScale = 0f; // Pause the game
            mcqPanel.SetActive(true); // Show the MCQ
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    public void HandleMCQAnswer(string answer)
    {
        if (answer == "3") // Correct answer
        {
            Time.timeScale = 1f;
            ScoreManager.Instance.EnterSecondLevel();
            mcqPanel.SetActive(false);// Name of second level scene
        }
        else
        {
            Debug.Log("Wrong answer!");
            // Optional: show feedback or retry logic
        }
    }
}
