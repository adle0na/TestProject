using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecyclePopupNoneABB : MonoBehaviour
{
    [Header("팝업 제목과 내용")] public TMP_Text titleText;
    public TMP_Text descriptionText;

    [Header("예 버튼")] public Button agreeBtn;
    public TMP_Text agreeBtnText;

    [Header("아니오 버튼")] public Button disagreeBtn;
    public TMP_Text disagreeBtnText;

    public Action YAction;
    public Action NAction;

    public void SetYButtonAction()
    {
        agreeBtn.onClick.AddListener(() => YAction());
    }
    
    public void SetNButtonAction()
    {
        disagreeBtn.onClick.AddListener(() => NAction());
    }
}
