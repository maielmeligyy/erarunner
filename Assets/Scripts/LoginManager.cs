using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("Input Fields")]
    public InputField usernameField;
    public InputField passwordField;

    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject signupPanel;

    private void Start()
    {
        
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
    }

    public void OnLoginPressed()
    {
        Debug.Log("Login Button Clicked");

        if (usernameField.text.Trim().ToLower() == "test" && passwordField.text == "1234")
        {
            Debug.Log("Login successful");
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.LogWarning("Wrong credentials entered");
        }
    }

    public void OnSignupPressed()
    {
        Debug.Log("Switched to Signup Panel");
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
    }

    public void OnBackToLoginPressed()
    {
        Debug.Log("Switched back to Login Panel");
        signupPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void OnRegisterPressed()
    {
        
        Debug.Log("Fake register complete — returning to login");
        signupPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
}
