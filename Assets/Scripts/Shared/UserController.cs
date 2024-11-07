using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    protected UserModel user;
    private DatabaseManager db;

    void Start()
    {
        string userJson = PlayerPrefs.GetString("user", string.Empty);
        if (!string.IsNullOrEmpty(userJson))
        {
            user = JsonUtility.FromJson<UserModel>(userJson);
        }
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
        StartCoroutine(this.HandleSaveUser());
    }
}
