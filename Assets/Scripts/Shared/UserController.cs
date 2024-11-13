using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    protected UserModel user;
    private DatabaseManager db;

    void Awake()
    {
        string userJson = PlayerPrefs.GetString("user", string.Empty);

        if (!string.IsNullOrEmpty(userJson))
        {
            Debug.Log("userJson" + userJson);

            user = JsonUtility.FromJson<UserModel>(userJson);

            Debug.Log("user" + user);
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
}
