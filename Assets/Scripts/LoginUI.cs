using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginUI : MonoBehaviour
{
    public TMPro.TMP_InputField usernameInput;
    public TMPro.TMP_InputField passwordInput;
    public DatabaseManager dbManager;

    public void OnLoginButtonPressed()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (dbManager.LoginUser(username, password))
        {
            Debug.Log("Login successful!");
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("Invalid username or password");
        }
    }

}
