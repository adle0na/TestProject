using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecyclePopupABB : MonoBehaviour
{
    [Header("팝업 제목과 내용")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    
    [Header("확인 버튼")]
    public Button agreeBtn;
    public TMP_Text agreeBtnText;
    public Action ButtonClicked;

    public void SetButtonAction()
    {
        agreeBtn.onClick.AddListener(() => ButtonClicked());
    }
}