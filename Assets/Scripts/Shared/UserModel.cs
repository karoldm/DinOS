using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class UserModel
{
    private string username;
    private string password;

    public UserModel(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public UserModel()
    {
        this.username = "";
        this.password = "";
    }

    public static UserModel FromSnapshot(DataSnapshot snapshot)
    {
        if (snapshot == null || !snapshot.Exists)
        {
            return null; 
        }

        UserModel user = new UserModel
        {
            username = snapshot.Child("username").Value?.ToString(),
            password = snapshot.Child("password").Value?.ToString()
        };

        return user;
    }

    public string GetUsername()
    {
        return this.username;
    }

    public string GetPassword()
    {
        return this.password;
    }
        
    public void SetUsername(string username)
    {
        this.username = username;
    }

    public void SetPassword(string password)
    {
        this.password = password;
    }
}
