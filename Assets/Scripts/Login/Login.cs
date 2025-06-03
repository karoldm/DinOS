using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    private DatabaseManager db;
    private UserModel user = new UserModel(username: "", password: "");
    private int attempts = 0;

    public GameObject aboutModal;
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public GameObject wrongPasswordMessage;
    public GameObject emptyDataMessage;

    void Start()
    {
        db = FindFirstObjectByType<DatabaseManager>();
        if (db == null)
        {
            Debug.LogError("DatabaseManager instance not found in the scene.");
        }

        usernameField.Select();
        usernameField.ActivateInputField();
    }

    public void OnChangeUsername()
    {
        this.user.username = usernameField.text;
    }

    public void OnChangePassword()
    {
        this.user.password = passwordField.text;
    }

    public void OpenAboutModal()
    {
        this.aboutModal.SetActive(true);
    }

    public void CloseAboutModal()
    {
        this.aboutModal.SetActive(false);
    }

    private void LoginUser(UserModel dbUser)
    {
        PlayerPrefs.SetString("user", JsonUtility.ToJson(dbUser));
        SceneManager.LoadScene("Home");
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
                TryLogin();
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

        if (string.IsNullOrWhiteSpace(user.username) || string.IsNullOrWhiteSpace(user.password))
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
