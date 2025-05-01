using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SignUpUI : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text resultText;

    private void Start()
    {
        if (emailInput == null || passwordInput == null || resultText == null)
        {
            Debug.LogError("SignUpUI: UI references are missing in Inspector!");
        }
    }

    public void OnRegisterButtonPressed()
    {
        Debug.Log("Sign up button pressed.");

        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        Debug.Log("Email entered: " + email);
        Debug.Log("Password entered: " + password);

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            resultText.text = "Email and password required.";
            return;
        }

        if (!IsValidEmail(email))
        {
            resultText.text = "Invalid email format.";
            return;
        }

        if (password.Length < 6)
        {
            resultText.text = "Password must be at least 6 characters.";
            return;
        }
        if (DatabaseManager.Instance == null)
        {
            Debug.LogError(" DatabaseManager.Instance is NULL! The script might not be in the scene.");
            resultText.text = "Signup system is not ready. Please check setup.";
            return;
        }

        StartCoroutine(DatabaseManager.Instance.RegisterUser(email, password, (response) =>
        {
            Debug.Log("Register response: " + response);

            if (response.StartsWith("{"))
            {
                resultText.text = "Registered successfully!";
                SceneManager.LoadScene("LoginScene"); // Go to login screen
            }
            else
            {
                resultText.text = "Error: " + response;
            }
        }));
    }

    private bool IsValidEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
