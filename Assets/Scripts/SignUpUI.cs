using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SignUpUI : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_Text resultText;

    private void Start()
    {
        if (usernameInput == null || emailInput == null || resultText == null)
        {
            Debug.LogError("SignUpUI: UI references are missing in Inspector!");
        }
    }

    public void OnRegisterButtonPressed()
    {
        Debug.Log("Sign up button pressed.");

        string username = usernameInput.text.Trim();
        string email = emailInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
        {
            resultText.text = "Username and email required.";
            Debug.LogWarning("Signup failed: empty input.");
            return;
        }

        StartCoroutine(DatabaseManager.Instance.RegisterUser(username, email, (response) =>
        {
            Debug.Log("Register response: " + response);

            if (response.StartsWith("{"))
            {
                resultText.text = "Registered successfully!";
                SceneManager.LoadScene("SampleScene"); 
            }
            else
            {
                resultText.text = "Error: " + response;
            }
        }));
    }
}
