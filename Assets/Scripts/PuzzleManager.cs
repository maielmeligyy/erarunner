using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle UI")]
    public GameObject memoryMatchPuzzle;
    public GameObject quickTimeEventPuzzle;
    public GameObject quizPuzzle;
    public GameObject patternSequencePuzzle;
    public Text puzzleInstructionText;
    public Text countdownText;
    public Button[] memoryMatchButtons;
    public Button[] quickTimeButtons;
    public Button[] quizButtons;
    public Button[] sequenceButtons;
    
    [Header("Puzzle Content")]
    [TextArea(3, 5)]
    public string[] eraQuizQuestions;
    public string[][] eraQuizAnswers; // Array of arrays - each sub-array holds 4 possible answers
    public int[] eraQuizCorrectAnswers; // Index of correct answer for each quiz
    
    // References
    private GameManager gameManager;
    private GameObject currentActivePuzzle;
    private bool puzzleActive = false;
    private bool puzzleSolved = false;
    private float puzzleTimeLimit = 10f;
    private float puzzleTimer;
    
    // For memory match
    private int[] memoryValues;
    private int firstSelected = -1;
    private int secondSelected = -1;
    private int pairsFound = 0;
    private int totalPairs = 3;
    
    // For pattern sequence
    private List<int> currentSequence = new List<int>();
    private List<int> playerSequence = new List<int>();
    private int sequenceLength = 4;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        HideAllPuzzles();
    }
    
    public void StartPuzzle(GameManager.PuzzleType puzzleType, int eraIndex)
    {
        puzzleActive = true;
        puzzleSolved = false;
        puzzleTimer = puzzleTimeLimit;
        
        HideAllPuzzles();
        
        switch (puzzleType)
        {
            case GameManager.PuzzleType.MemoryMatch:
                SetupMemoryMatch();
                currentActivePuzzle = memoryMatchPuzzle;
                puzzleInstructionText.text = "Match the pairs before time runs out!";
                break;
                
            case GameManager.PuzzleType.QuickTimeEvent:
                SetupQuickTimeEvent();
                currentActivePuzzle = quickTimeEventPuzzle;
                puzzleInstructionText.text = "Press the highlighted buttons quickly!";
                break;
                
            case GameManager.PuzzleType.SimpleQuiz:
                SetupSimpleQuiz(eraIndex);
                currentActivePuzzle = quizPuzzle;
                puzzleInstructionText.text = "Answer the question about this era!";
                break;
                
            case GameManager.PuzzleType.PatternSequence:
                SetupPatternSequence();
                currentActivePuzzle = patternSequencePuzzle;
                puzzleInstructionText.text = "Repeat the pattern you see!";
                break;
        }
        
        if (currentActivePuzzle != null)
            currentActivePuzzle.SetActive(true);
        
        StartCoroutine(PuzzleCountdown());
    }
    
    private void HideAllPuzzles()
    {
        if (memoryMatchPuzzle != null) memoryMatchPuzzle.SetActive(false);
        if (quickTimeEventPuzzle != null) quickTimeEventPuzzle.SetActive(false);
        if (quizPuzzle != null) quizPuzzle.SetActive(false);
        if (patternSequencePuzzle != null) patternSequencePuzzle.SetActive(false);
    }
    
    private IEnumerator PuzzleCountdown()
    {
        while (puzzleTimer > 0 && puzzleActive && !puzzleSolved)
        {
            puzzleTimer -= Time.unscaledDeltaTime;
            
            if (countdownText != null)
                countdownText.text = "Time: " + Mathf.CeilToInt(puzzleTimer).ToString();
            
            yield return null;
        }
        
        if (!puzzleSolved && puzzleActive)
        {
            // Puzzle failed due to timeout
            CompletePuzzle(false);
        }
    }
    
    public void CompletePuzzle(bool success)
    {
        puzzleActive = false;
        puzzleSolved = success;
        
        // Call back to game manager
        if (gameManager != null)
        {
            // Pass the result back to GameManager
            // This will be implemented in the GameManager
            // gameManager.PuzzleCompleted(success);
        }
        
        HideAllPuzzles();
    }
    
    #region Memory Match Puzzle
    private void SetupMemoryMatch()
    {
        // Reset puzzle state
        pairsFound = 0;
        firstSelected = -1;
        secondSelected = -1;
        
        // Create the pairs (values 0-2, each appears twice)
        memoryValues = new int[6] { 0, 0, 1, 1, 2, 2 };
        
        // Shuffle the values
        for (int i = 0; i < memoryValues.Length; i++)
        {
            int temp = memoryValues[i];
            int randomIndex = Random.Range(i, memoryValues.Length);
            memoryValues[i] = memoryValues[randomIndex];
            memoryValues[randomIndex] = temp;
        }
        
        // Reset all buttons
        for (int i = 0; i < memoryMatchButtons.Length && i < memoryValues.Length; i++)
        {
            int buttonIndex = i; // Need this to capture for lambda
            
            // Reset button appearance
            memoryMatchButtons[i].GetComponentInChildren<Text>().text = "?";
            memoryMatchButtons[i].interactable = true;
            
            // Add click handler
            memoryMatchButtons[i].onClick.RemoveAllListeners();
            memoryMatchButtons[i].onClick.AddListener(() => OnMemoryButtonClicked(buttonIndex));
        }
    }
    
    private void OnMemoryButtonClicked(int index)
    {
        if (firstSelected == -1)
        {
            // First card turned
            firstSelected = index;
            memoryMatchButtons[index].GetComponentInChildren<Text>().text = memoryValues[index].ToString();
            memoryMatchButtons[index].interactable = false;
        }
        else if (secondSelected == -1 && index != firstSelected)
        {
            // Second card turned
            secondSelected = index;
            memoryMatchButtons[index].GetComponentInChildren<Text>().text = memoryValues[index].ToString();
            memoryMatchButtons[index].interactable = false;
            
            // Check if it's a match
            StartCoroutine(CheckMemoryMatch());
        }
    }
    
    private IEnumerator CheckMemoryMatch()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        
        if (memoryValues[firstSelected] == memoryValues[secondSelected])
        {
            // Match found
            pairsFound++;
            
            // Check if all pairs are found
            if (pairsFound >= totalPairs)
            {
                CompletePuzzle(true);
            }
        }
        else
        {
            // No match, flip cards back
            memoryMatchButtons[firstSelected].GetComponentInChildren<Text>().text = "?";
            memoryMatchButtons[secondSelected].GetComponentInChildren<Text>().text = "?";
            memoryMatchButtons[firstSelected].interactable = true;
            memoryMatchButtons[secondSelected].interactable = true;
        }
        
        // Reset selections
        firstSelected = -1;
        secondSelected = -1;
    }
    #endregion
    
    #region Quick Time Event Puzzle
    private void SetupQuickTimeEvent()
    {
        StartCoroutine(RunQuickTimeSequence());
    }
    
    private IEnumerator RunQuickTimeSequence()
    {
        int buttonsPressed = 0;
        int requiredButtonPresses = 5;
        int activeButtonIndex = -1;
        
        while (buttonsPressed < requiredButtonPresses && puzzleActive && !puzzleSolved)
        {
            // Deactivate previous button if any
            if (activeButtonIndex >= 0 && activeButtonIndex < quickTimeButtons.Length)
            {
                quickTimeButtons[activeButtonIndex].GetComponent<Image>().color = Color.white;
                quickTimeButtons[activeButtonIndex].onClick.RemoveAllListeners();
            }
            
            // Activate a random button
            activeButtonIndex = Random.Range(0, quickTimeButtons.Length);
            quickTimeButtons[activeButtonIndex].GetComponent<Image>().color = Color.green;
            
            // Add click handler for success
            int currentIndex = activeButtonIndex; // Capture for lambda
            quickTimeButtons[currentIndex].onClick.RemoveAllListeners();
            quickTimeButtons[currentIndex].onClick.AddListener(() => {
                buttonsPressed++;
                quickTimeButtons[currentIndex].GetComponent<Image>().color = Color.white;
            });
            
            // Wait for reaction time
            float reactionTime = 1.0f - (buttonsPressed * 0.15f); // Gets faster with each success
            reactionTime = Mathf.Max(reactionTime, 0.3f); // Minimum reaction time
            
            yield return new WaitForSecondsRealtime(reactionTime);
            
            // Check if button was pressed
            if (quickTimeButtons[activeButtonIndex].GetComponent<Image>().color == Color.green)
            {
                // Button not pressed in time - fail
                CompletePuzzle(false);
                yield break;
            }
        }
        
        // All buttons pressed successfully
        if (buttonsPressed >= requiredButtonPresses)
        {
            CompletePuzzle(true);
        }
    }
    #endregion
    
    #region Simple Quiz Puzzle
    private void SetupSimpleQuiz(int eraIndex)
    {
        // Safety check
        if (eraIndex >= eraQuizQuestions.Length)
        {
            eraIndex = 0;
        }
        
        // Set question text
        Text questionText = quizPuzzle.GetComponentInChildren<Text>();
        if (questionText != null)
        {
            questionText.text = eraQuizQuestions[eraIndex];
        }
        
        // Set answer buttons
        for (int i = 0; i < quizButtons.Length && i < eraQuizAnswers[eraIndex].Length; i++)
        {
            Text buttonText = quizButtons[i].GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = eraQuizAnswers[eraIndex][i];
            }
            
            int buttonIndex = i; // Capture for lambda
            quizButtons[i].onClick.RemoveAllListeners();
            quizButtons[i].onClick.AddListener(() => OnQuizAnswerSelected(eraIndex, buttonIndex));
        }
    }
    
    private void OnQuizAnswerSelected(int eraIndex, int answerIndex)
    {
        // Check if answer is correct
        bool correct = (answerIndex == eraQuizCorrectAnswers[eraIndex]);
        CompletePuzzle(correct);
    }
    #endregion
    
    #region Pattern Sequence Puzzle
    private void SetupPatternSequence()
    {
        // Clear previous sequences
        currentSequence.Clear();
        playerSequence.Clear();
        
        // Generate random sequence
        for (int i = 0; i < sequenceLength; i++)
        {
            currentSequence.Add(Random.Range(0, sequenceButtons.Length));
        }
        
        // Show the sequence to player
        StartCoroutine(ShowSequence());
    }
    
    private IEnumerator ShowSequence()
    {
        // Disable all buttons during sequence display
        foreach (Button button in sequenceButtons)
        {
            button.interactable = false;
        }
        
        yield return new WaitForSecondsRealtime(1.0f);
        
        // Flash each button in sequence
        for (int i = 0; i < currentSequence.Count; i++)
        {
            int buttonIndex = currentSequence[i];
            
            // Flash button
            sequenceButtons[buttonIndex].GetComponent<Image>().color = Color.yellow;
            yield return new WaitForSecondsRealtime(0.5f);
            sequenceButtons[buttonIndex].GetComponent<Image>().color = Color.white;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        
        // Enable buttons and set up listeners for player input
        for (int i = 0; i < sequenceButtons.Length; i++)
        {
            int buttonIndex = i; // Capture for lambda
            sequenceButtons[i].interactable = true;
            sequenceButtons[i].onClick.RemoveAllListeners();
            sequenceButtons[i].onClick.AddListener(() => OnSequenceButtonPressed(buttonIndex));
        }
        
        // Instructions
        puzzleInstructionText.text = "Now repeat the sequence!";
    }
    
    private void OnSequenceButtonPressed(int buttonIndex)
    {
        // Flash button
        StartCoroutine(FlashButton(buttonIndex));
        
        // Add to player sequence
        playerSequence.Add(buttonIndex);
        
        // Check if sequence is correct so far
        int playerIndex = playerSequence.Count - 1;
        if (playerSequence[playerIndex] != currentSequence[playerIndex])
        {
            // Wrong button pressed
            CompletePuzzle(false);
            return;
        }
        
        // Check if full sequence completed
        if (playerSequence.Count == currentSequence.Count)
        {
            // Completed successfully
            CompletePuzzle(true);
        }
    }
    
    private IEnumerator FlashButton(int buttonIndex)
    {
        sequenceButtons[buttonIndex].GetComponent<Image>().color = Color.green;
        yield return new WaitForSecondsRealtime(0.3f);
        sequenceButtons[buttonIndex].GetComponent<Image>().color = Color.white;
    }
    #endregion
} 