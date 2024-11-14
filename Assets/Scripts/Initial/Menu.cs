using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : UserController
{
    public TextMeshProUGUI usernameMenu;
    public ProgressBar progressBarMenu;


    // Start is called before the first frame update
    void Start()
    {
        usernameMenu.text = user.username;
        int unlockedAwards = user.GetAwardsAmount();
        float percent = unlockedAwards / 8f;
        progressBarMenu.UpdateProgressBar(percent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Initial");
    }
}
