using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Login : MonoBehaviour
{
    private DatabaseManager db;
    private UserModel user = new UserModel();
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public GameObject wrongPasswordMessage;
    public GameObject emptyDataMessage;

    void Start()
    {
        db = FindObjectOfType<DatabaseManager>();
        if (db == null)
        {
            Debug.LogError("DatabaseManager instance not found in the scene.");
        }
    }

    void Update()
    {
        
    }

    public void OnChangeUsername()
    {
        this.user.SetUsername(usernameField.text);
    }

    public void OnChangePassword()
    {
        this.user.SetPassword(passwordField.text);

    }

    private void LoginUser(UserModel dbUser)
    {
        // get all data from firebase and set on playerprefs to use in other screens
        Debug.Log("User logged!");
    }

    private void HandleWrongPassword()
    {
        wrongPasswordMessage.SetActive(true);
    }

    private async void HandleCreateUser()
    {
        await db.CreateUser(user);
        // this.TryLogin();
    }

    private void HandleEmptyData()
    {
        emptyDataMessage.SetActive(true);
    }

    private void ClearErrorMessages()
    {
        wrongPasswordMessage.SetActive(false);
        emptyDataMessage.SetActive(false);
    }

    public async void TryLogin()
    {
        this.ClearErrorMessages();

        if(user.GetUsername() == "" || user.GetPassword() == "")
        {
            this.HandleEmptyData();
            return;
        }

        UserModel dbUser = await db.ReadUserOrNull(user.GetUsername());
        if (dbUser != null)
        {
            if (user.GetPassword() == dbUser.GetPassword())
            {
                this.LoginUser(dbUser);
            }
            else
            {
                this.HandleWrongPassword();
            }
        }
        else
        {
            this.HandleCreateUser();
        }
    }
}
