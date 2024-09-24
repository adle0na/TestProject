using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickNameCreatePopup : MonoBehaviour
{
    public bool isChange;
    [SerializeField] private TMP_InputField nickNameInput;
    
    public void CheckNickNameUsable()
    {
        List<InAppositeWordData> appositeWordDatas = GoogleSheetManager.Instance.inAppositeWordDB;
        
        foreach (var word in appositeWordDatas)
        {
            if (nickNameInput.text.Contains(word.word))
            {
                UIManager.Instance.OpenRecyclePopupOneButton("시스템 메세지", $"사용 불가능한 단어가 포함되어 있습니다.{word.word}", null, "");
            }
        }

        //Action nextAction = () => BackendManager.Instance.ChangeNickName(nickNameInput.text, isChange);
        
        //UIManager.Instance.OpenRecyclePopupTwoButton("시스템 메세지", $"사용 가능합니다. \n {nickNameInput.text}으로 하시겠습니까?", nextAction, null);
    }
}
