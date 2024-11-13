using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : UserController
{
    public ProfileModal modal;
    public TextMeshProUGUI usernameMenu;
    public ProgressBar progressBarMenu;

    // Start is called before the first frame update
    void Start()
    {
        usernameMenu.text = user.username;
        int unlockedAwards = user.levelOne.awards.Count + user.levelTwo.awards.Count + user.levelThree.awards.Count + user.levelFour.awards.Count;
        float percent = unlockedAwards / 8f;
        progressBarMenu.UpdateProgressBar(percent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenModal()
    {
        this.modal.gameObject.SetActive(true);
    }

    public void CloseModal()
    {
        this.modal.gameObject.SetActive(false);
        this.modal.CloseAllContents();
    }

    public void Exit()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Initial");
    }
}
