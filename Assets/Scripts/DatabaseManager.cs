using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    private string serverUrl = "http://localhost:3000"; // Change to your IP when testing on mobile

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator RegisterUser(string email, string password, Action<string> callback)
    {
        string jsonData = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(serverUrl + "/register", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                callback(request.downloadHandler.text);
            else
                callback("Error: " + request.downloadHandler.text);
        }
    }
    public IEnumerator LoginUser(string email, string password, Action<string> callback)
    {
        string jsonData = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(serverUrl + "/login", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback(request.downloadHandler.text);
            }
            else
            {
                callback("Error: " + request.downloadHandler.text);
            }
        }
    }

}
