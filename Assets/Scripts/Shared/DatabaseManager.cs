using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                try
                {
                    AppOptions options = new AppOptions()
                    {
                        ApiKey = ENV.API_KEY,
                        AppId = ENV.APP_ID,
                        ProjectId = ENV.PROJECT_ID,
                        DatabaseUrl = new System.Uri(ENV.DATABASE_URL)
                    };

                    FirebaseApp app = FirebaseApp.Create(options, "DinoSO");
                    databaseReference = FirebaseDatabase.GetInstance(app).RootReference;
                    Debug.Log("Firebase Realtime Database initialized!");

                } catch(Exception ex)
                {
                    Debug.LogError("Error on initialize firebase: " + ex);
                }
            }
            else
            {
                Debug.LogError("Error on initialize firebase: " + task.Result);
            }
        });
    }

    public async Task<UserModel> ReadUserOrNull(string username)
    {
        try
        {
            DataSnapshot snapshot = await databaseReference.Child("users").Child(username).GetValueAsync();

            if (snapshot.Exists)
            {
                return UserModel.FromSnapshot(snapshot);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error on read data: " + ex);
            return null;
        }
    }

    public async Task CreateUser(UserModel user)
    {
        if (user == null || string.IsNullOrEmpty(user.GetUsername()) || string.IsNullOrEmpty(user.GetPassword()))
        {
            Debug.LogError("User or required fields are null or empty.");
            return;
        }

        try
        {
            await databaseReference.Child("users").Child(user.GetUsername()).SetRawJsonValueAsync(JsonUtility.ToJson(user));
            Debug.Log("User created successfully!");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error on create user: " + ex);
        }
    }

}
