using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileModal : UserController
{
    public GameObject faseOneButton;
    public GameObject faseTwoButton;
    public GameObject faseThreeButton;
    public GameObject faseFourButton;

    public GameObject faseOneContent;
    public GameObject faseTwoContent;
    public GameObject faseThreeContent;
    public GameObject faseFourContent;

    public TextMeshProUGUI scoreLevelTwo;
    public TextMeshProUGUI scoreLevelThree;
    public TextMeshProUGUI scoreLevelFour;
    public TextMeshProUGUI usernameModal;
    public TextMeshProUGUI lastDateLogged;

    public ProgressBar progressBarModal;

    void Start()    
    {
        usernameModal.text = user.username;
        lastDateLogged.text = user.lastDateLogged;
        InitProgressBar();
        InitScores();
        InitAwards();  
    }


    private void InitProgressBar()
    {
        int unlockedAwards = user.levelOne.awards.Count + user.levelTwo.awards.Count + user.levelThree.awards.Count + user.levelFour.awards.Count;
        float percent = unlockedAwards / 8f;
        progressBarModal.UpdateProgressBar(percent);
    }

    private void InitAwards()
    {
        Award[] awardsLevelOne = faseOneContent.GetComponentsInChildren<Award>();
        foreach (Award award in awardsLevelOne)
        {
            if (user.levelOne.awards.Contains(award.gameObject.name))
            {
                award.Unlock();
            }
        }

        Award[] awardsLevelTwo = faseTwoContent.GetComponentsInChildren<Award>();
        foreach (Award award in awardsLevelTwo)
        {
            if (user.levelTwo.awards.Contains(award.gameObject.name))
            {
                award.Unlock();
            }
        }

        Award[] awardsLevelThree = faseThreeContent.GetComponentsInChildren<Award>();
        foreach (Award award in awardsLevelThree)
        {
            if (user.levelThree.awards.Contains(award.gameObject.name))
            {
                award.Unlock();
            }
        }

        Award[] awardsLevelFour = faseFourContent.GetComponentsInChildren<Award>();
        foreach (Award award in awardsLevelFour)
        {
            if (user.levelFour.awards.Contains(award.gameObject.name))
            {
                award.Unlock();
            }
        }
    }

    private void InitScores()
    {

        if (scoreLevelTwo != null)
        {
            scoreLevelTwo.text = user.levelTwo.score.ToString();
        }

        if (scoreLevelThree != null)
        {
            scoreLevelThree.text = user.levelThree.score.ToString();
        }

        if (scoreLevelFour != null)
        {
            scoreLevelFour.text = user.levelFour.score.ToString();
        }
    }


    void Update()
    {
        
    }

    private void ChangeButtonColor(GameObject button, Color color)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }
        else
        {
            Debug.LogError("Image component missing on the button.");
        }
    }

    public void CloseAllContents()
    {
        faseOneContent.SetActive(false);
        faseTwoContent.SetActive(false);
        faseThreeContent.SetActive(false);
        faseFourContent.SetActive(false);
    }

    public void onClickFaseOneButton()
    {
        this.CloseAllContents();
        this.faseOneContent.SetActive(true);
    }

    public void onClickFaseTwoButton()
    {
        this.CloseAllContents();
        this.faseTwoContent.SetActive(true);
    }

    public void onClickFaseThreeButton()
    {
        this.CloseAllContents();
        this.faseThreeContent.SetActive(true);
    }

    public void onClickFaseFourButton()
    {
        this.CloseAllContents();
        this.faseFourContent.SetActive(true);
    }
}
