using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class UserController : MonoBehaviour
{
    protected UserModel user;
    private DatabaseManager db;

    void Awake()
    {
        string userJson = PlayerPrefs.GetString("user", string.Empty);

        if (!string.IsNullOrEmpty(userJson))
        {
            user = JsonUtility.FromJson<UserModel>(userJson);
        }
        else
        {
            Debug.LogError("Failed to load user from playerPrefs.");
        }
    }

    void Start()
    {
       
    }

    void Update()
    {
        
    }

    private IEnumerator HandleSaveUser()
    {
        yield return StartCoroutine(db.SaveUser(user, success => { }));
    }

    protected void UpdateUser()
    {
        db = DatabaseManager.Instance;

        if (db != null)
        {
            StartCoroutine(this.HandleSaveUser());
        }
        else
        {
            Debug.LogError("DatabaseManager instance is null. Cannot update user.");
        }
    }

    protected void GetAllUsers(Action<List<UserModel>> onComplete, Action onFailure)
    {
        db = DatabaseManager.Instance;

        if (db != null)
        {
            StartCoroutine(db.GetAllUsers(
                listUsers =>
                {
                    onComplete?.Invoke(listUsers); 
                },
                () =>
                {
                    Debug.LogError("Failed to retrieve users.");
                    onFailure?.Invoke(); 
                }
            ));
        }
        else
        {
            Debug.LogError("DatabaseManager instance is null. Cannot get all users.");
            onFailure?.Invoke();
        }
    }
}
