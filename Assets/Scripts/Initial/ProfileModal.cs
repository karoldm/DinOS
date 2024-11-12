using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Color buttonColor = new Color(255, 219, 174);
    private Color buttonSelectedColor = new Color(241, 184, 112);

    public ProgressBar progressBar;

    void Start()    
    {
        int unlockedAwards = user.levelOne.awards.Count + user.levelTwo.awards.Count + user.levelThree.awards.Count + user.levelFour.awards.Count;
        float percent = unlockedAwards / 8;
        progressBar.UpdateProgressBar(percent);

        Award[] awardsLevelOne = faseOneContent.GetComponentsInChildren<Award>();
        foreach(Award award in awardsLevelOne)
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
