using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }
    private string serverUrl = "http://localhost:3000";

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public IEnumerator RegisterUser(string username, string email, Action<string> callback)
    {
        string jsonData = $"{{\"username\":\"{username}\", \"email\":\"{email}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(serverUrl + "/register", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                callback(request.downloadHandler.text);
            else
                callback($"Error: {request.error}");
        }
    }

    public IEnumerator LoginUser(int id, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl + $"/user/{id}"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                callback(request.downloadHandler.text);
            else
                callback($"Error: {request.error}");
        }
    }
}
