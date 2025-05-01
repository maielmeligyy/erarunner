using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text resultText;

    public void OnLoginButtonPressed()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            resultText.text = "Enter both email and password.";
            return;
        }

        if (DatabaseManager.Instance == null)
        {
            resultText.text = "Login system not ready. Try again later.";
            return;
        }

        StartCoroutine(DatabaseManager.Instance.LoginUser(email, password, (response) =>
        {
            Debug.Log("Login response: " + response);

            if (response.Contains("Login successful"))
            {
                resultText.text = "Login successful!";
                SceneManager.LoadScene("SampleScene"); 
            }
            else
            {
                resultText.text = "Invalid email or password.";
            }
        }));
    }

    public void OnSignUpButtonPressed()
    {
        SceneManager.LoadScene("SignUpScene");
    }
}
