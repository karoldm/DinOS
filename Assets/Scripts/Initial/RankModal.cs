using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RankModal : UserController
{
    public GameObject contentRankRows;

    void Start()
    {
        GetAllUsers(
           onComplete: users =>
           {
               if (users != null)
               {
                   var sortedUsers = users.OrderByDescending(user => user.GetAwardsAmount())
                                          .ThenByDescending(user => user.levelOne.score + user.levelTwo.score + user.levelThree.score + user.levelFour.score)
                                          .ToList();

                   foreach (var user in sortedUsers)
                   {
                       this.CreateNewRankRow(user.username, user.levelTwo.score, user.levelThree.score, user.levelFour.score, user.GetAwardsAmount());
                   }
               }
           },
           onFailure: () =>
           {
               Debug.LogError("Failed to retrieve users.");
           }
       );
    }

    void Update()
    {

    }

    private void CreateNewRankRow(string username, int scoreFaseII, int scoreFaseIII, int scoreFaseIV, int awardsAmount)
    {
        GameObject horizontalLayout = new GameObject("HorizontalLayout", typeof(RectTransform));
        horizontalLayout.transform.SetParent(contentRankRows.transform);
        RectTransform rectTransform = horizontalLayout.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(400, 32); 
        }

        HorizontalLayoutGroup layoutGroup = horizontalLayout.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.spacing = 64;

        AddText(username, horizontalLayout);
        AddText(scoreFaseII.ToString(), horizontalLayout);
        AddText(scoreFaseIII.ToString(), horizontalLayout);
        AddText(scoreFaseIV.ToString(), horizontalLayout);
        AddText(awardsAmount.ToString(), horizontalLayout);
    }

    public void AddText(string text, GameObject horizontalLayout)
    {
        GameObject textObj = new GameObject(text, typeof(RectTransform));
        textObj.transform.SetParent(horizontalLayout.transform);

        TextMeshProUGUI textMeshPro = textObj.AddComponent<TextMeshProUGUI>();
        textMeshPro.text = text;
        textMeshPro.fontSize = 18;
        textMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts/PressStart2P-Regular SDF");
        textMeshPro.color = Color.black;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(90, 32);
    }

    public void OpenRankModal()
    {
        gameObject.SetActive(true);
    }

    public void CloseRankModal()
    {
        gameObject.SetActive(false);
    }
}
