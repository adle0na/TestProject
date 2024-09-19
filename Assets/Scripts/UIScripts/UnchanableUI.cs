using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnchangableUI : MonoBehaviour
{
    [Header("유저 정보")]
    public Image userProfileIcon;
    public TextMeshProUGUI userNickName;
    public TextMeshProUGUI zenyAmountText;
    public TextMeshProUGUI gemAmountText;

    public void ChangeUI(int index)
    {
        //UIManager.Instance.sceneController.GetComponent<MainSceneController>().UIChange(index);
    }
    
    public void OpenSettingPopup()
    {
        GameObject settingPopup = UIManager.Instance.SettingPopupPrefab;

        UIManager.Instance.OpenPopup(settingPopup);
    }
    
    public void OpenNotReadyPopup()
    {
        UIManager.Instance.OpenRecyclePopupOneButton("시스템 메세지", "아직 준비 되지 않았어요..", null, "");
    }
}