using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_Text resultText;

    private void Start()
    {
        if (idInput == null || resultText == null)
        {
            Debug.LogError("LoginUI: Missing UI references!");
        }
    }

    public void OnLoginButtonPressed()
    {
        Debug.Log("Login button pressed");

        if (string.IsNullOrWhiteSpace(idInput.text))
        {
            resultText.text = "ID field is empty.";
            Debug.LogWarning("Login failed: ID field is empty.");
            return;
        }

        if (int.TryParse(idInput.text.Trim(), out int userId))
        {
            Debug.Log("Attempting login with ID: " + userId);

            StartCoroutine(DatabaseManager.Instance.LoginUser(userId, (response) =>
            {
                Debug.Log("Backend response: " + response);

                if (response.StartsWith("{")) // crude check: if it's a valid JSON object
                {
                    resultText.text = "Login successful!";
                    SceneManager.LoadScene("SampleScene");
                }
                else
                {
                    resultText.text = "Login failed: " + response;
                }
            }));
        }
        else
        {
            resultText.text = "Invalid ID format.";
            Debug.LogWarning("Login failed: ID is not a number.");
        }
    }
}
