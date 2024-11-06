using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserModel
{
    public string username;
    public string password;

    public UserModel(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public static UserModel FromJson(string json)
    {
        return JsonUtility.FromJson<UserModel>(json);
    }
}
