using System.Collections;
using UnityEngine;
using TMPro;

public class Login : MonoBehaviour
{
    private DatabaseManager db;
    private UserModel user = new UserModel(username: "", password: "");
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public GameObject wrongPasswordMessage;
    public GameObject emptyDataMessage;
    private int attempts = 0;

    void Start()
    {
        db = FindObjectOfType<DatabaseManager>();
        if (db == null)
        {
            Debug.LogError("DatabaseManager instance not found in the scene.");
        }
    }

    public void OnChangeUsername()
    {
        this.user.username = usernameField.text;
    }

    public void OnChangePassword()
    {
        this.user.password = passwordField.text;
    }

    private void LoginUser(UserModel dbUser)
    {
        Debug.Log("User logged!");
        // Handle post-login actions here
    }

    private void HandleWrongPassword()
    {
        wrongPasswordMessage.SetActive(true);
    }

    private IEnumerator HandleCreateUser()
    {
        attempts++;
        yield return StartCoroutine(db.CreateUser(user, success =>
        {
            if (success)
            {
                TryLogin(); // Retry login after creating user
            }
            else if (attempts <= 3)
            {
                Debug.LogWarning("Retrying user creation...");
                StartCoroutine(HandleCreateUser());
            }
        }));
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

    public void TryLogin()
    {
        this.ClearErrorMessages();

        if (string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.password))
        {
            this.HandleEmptyData();
            return;
        }

        StartCoroutine(db.ReadUserOrNull(user.username, dbUser =>
        {
            if (dbUser != null)
            {
                if (user.password == dbUser.password)
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
                StartCoroutine(HandleCreateUser());
            }
        },
        () =>
        {
            Debug.LogError("Failed to read user data.");
        }));
    }
}
