using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    private static DatabaseManager instance;


    public static DatabaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DatabaseManager>();

                if (instance == null)
                {
                    Debug.LogError("No instance of DatabaseManager found in the scene.");
                }

            }
            return instance;
        }
    }


    private void Awake()
    {
       
    }

    private string databaseUrl = ENV.DATABASE_URL;

    public IEnumerator WriteData(string path, string jsonData, Action<bool> onComplete)
    {
        string url = $"{databaseUrl}{path}.json";
        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data written successfully");
                onComplete?.Invoke(true);
            }
            else
            {
                Debug.LogError("Error writing data: " + request.error);
                onComplete?.Invoke(false);
            }
        } 
    }

    public IEnumerator ReadData(string path, Action<string> onSuccess, Action<string> onFailure)
    {
        string url = $"{databaseUrl}{path}.json";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data received: " + request.downloadHandler.text);
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error reading data: " + request.error);
                onFailure?.Invoke(request.error);
            }
        } 
    }

    public IEnumerator CreateUser(UserModel user, Action<bool> onComplete)
    {
        string jsonData = JsonUtility.ToJson(user);
        yield return StartCoroutine(WriteData($"users/{user.username}", jsonData, onComplete));
    }

    public IEnumerator ReadUserOrNull(string username, Action<UserModel> onComplete, Action onFailure)
    {
        string path = $"users/{username}";

        yield return StartCoroutine(ReadData(path,
            jsonData =>
            {
                if (jsonData != "null")
                {
                    UserModel user = UserModel.FromJson(jsonData);
                    onComplete?.Invoke(user);
                }
                else
                {
                    onComplete?.Invoke(null);
                }
            },
            error =>
            {
                Debug.LogError("Error reading user: " + error);
                onFailure?.Invoke();
            }
        ));
    }

    public IEnumerator SaveUser(UserModel user, Action<bool> onComplete)
    {
        string jsonData = JsonUtility.ToJson(user);
        Debug.Log(user.levelOne.awards.ToString());
        Debug.Log("json data " + jsonData);

        yield return StartCoroutine(WriteData($"users/{user.username}", jsonData, onComplete));
        PlayerPrefs.SetString("user", jsonData);
    }
}
