using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserModel
{
    public string username;
    public string password;
    public LevelData levelOne;
    public LevelData levelTwo;
    public LevelData levelThree;
    public LevelData levelFour;

    public UserModel(string username, string password)
    {
        this.username = username;
        this.password = password;
        this.levelOne = new LevelData();
        this.levelTwo = new LevelData();
        this.levelThree = new LevelData();
        this.levelFour = new LevelData();
    }

    public UserModel(string username, string password, LevelData levelOne, LevelData levelTwo, LevelData levelThree, LevelData levelFour)
    {
        this.username = username;
        this.password = password;
        this.levelOne = levelOne;
        this.levelTwo = levelTwo;
        this.levelThree = levelThree;
        this.levelFour = levelFour;
    }

    public static UserModel FromJson(string json)
    {
        UserModel user = JsonUtility.FromJson<UserModel>(json);

        // Garante que todos os níveis sejam inicializados
        if (user.levelOne == null) user.levelOne = new LevelData();
        if (user.levelTwo == null) user.levelTwo = new LevelData();
        if (user.levelThree == null) user.levelThree = new LevelData();
        if (user.levelFour == null) user.levelFour = new LevelData();

        return user;
    }
}
