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
            rectTransform.sizeDelta = new Vector2(763, 32); 
        }

        HorizontalLayoutGroup layoutGroup = horizontalLayout.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = true;
        layoutGroup.spacing = 64;
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;

        AddText(username, horizontalLayout, true);
        AddText(scoreFaseII.ToString(), horizontalLayout, false);
        AddText(scoreFaseIII.ToString(), horizontalLayout, false);
        AddText(scoreFaseIV.ToString(), horizontalLayout, false);
        AddText(awardsAmount.ToString(), horizontalLayout, false);
    }

    public void AddText(string text, GameObject horizontalLayout, bool alignRight)
    {
        GameObject textObj = new GameObject(text, typeof(RectTransform));
        textObj.transform.SetParent(horizontalLayout.transform);

        TextMeshProUGUI textMeshPro = textObj.AddComponent<TextMeshProUGUI>();
        textMeshPro.text = text;
        textMeshPro.fontSize = 18;
        textMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts/PressStart2P-Regular SDF");
        textMeshPro.color = Color.black;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        textMeshPro.enableAutoSizing = false;

        textMeshPro.overflowMode = TextOverflowModes.Truncate; 
        textMeshPro.enableWordWrapping = false; 
        textMeshPro.alignment = alignRight ? TextAlignmentOptions.Right : TextAlignmentOptions.Center;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(100, 32); 
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.pivot = new Vector2(0.5f, 0.5f);

        LayoutElement layoutElement = textObj.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = 80; 
        layoutElement.preferredHeight = 32; 
        layoutElement.flexibleWidth = 0; 
        layoutElement.flexibleHeight = 0;
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
